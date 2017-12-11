using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Services;
using SandwichClub.Api.Middleware;
using SandwichClub.Api.Repositories.Models;
using Newtonsoft.Json.Serialization;
using GraphQL.Types;
using SandwichClub.Api.GraphQL;
using SandwichClub.Api.GraphQL.Types;
using System;
using System.IO;
using System.Linq;
using Microsoft.ApplicationInsights;
using SandwichClub.Api.GraphQL.Middleware;
using SandwichClub.Api.Migrations;

namespace SandwichClub.Api
{
    public class Startup
    {
        private string ConnectionString
            => Configuration.GetConnectionString("Database")
               ?? "Data Source=" + Path.Combine(Directory.GetCurrentDirectory(), "database.sqlite");

        public Startup(IHostingEnvironment env)
        {
            Console.WriteLine($"Running in {env.EnvironmentName} mode");

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var cs = ConnectionString;
            Console.WriteLine($"Using db: {cs}");

            services.AddDbContext<ScContext>(options => options.UseSqlite(cs).UseMemoryCache(null));

            InitializeAutoMapper();
            services.AddSingleton(Mapper.Instance);

            services.AddTransient<IAuthorisationService, FacebookAuthorisationService>();
            services.AddScoped<IScSession, ScSession>();

            services.AddScoped<IWeekRepository, WeekRepository>();
            services.AddScoped<IWeekService, WeekService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IWeekUserLinkRepository, WeekUserLinkRepository>();
            services.AddScoped<IWeekUserLinkService, WeekUserLinkService>();

            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            services.AddScoped<IGraphQLAuthenticationValidator, GraphQLAuthenticationValidator>();

            services.AddScoped<UserType>();
            services.AddScoped<WeekType>();
            services.AddScoped<WeekUserLinkType>();
            services.AddScoped<PaymentType>();
            services.AddScoped<SandwichClubQuery>();
            services.AddScoped<SandwichClubMutation>();
            services.AddScoped((sp) => new SandwichClubSchema(type => (GraphType) sp.GetService(type)));

            // Configs
            services.Configure<AuthenticationMiddlewareConfig>(Configuration);
            services.AddOptions();

            // Add framework services.
            services.AddCors();
            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, TelemetryClient telemetryClient)
        {
            // Migrate the database
            var context = app.ApplicationServices.GetService<ScContext>();
            ProcessDbMigrations(context);

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddProvider(new SqlDependencyLogProvider(Configuration.GetConnectionString("Database"), telemetryClient));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseGraphiQL(new GraphiQLOptions()
            {
                GraphiQLPath = "/graphiql"
            });

            app.UseMiddleware<AuthenticationMiddleware>();

            app.UseGraphQL(new GraphQLOptions
            {
                GraphQLPath = "/graphql" ,
            });

            app.UseMiddleware<AuthorizationMiddleware>();

            app.UseMvc();
        }

        public void ProcessDbMigrations(ScContext context)
        {
            var pendingMigrations = context.Database.GetPendingMigrations().ToList();
            if (pendingMigrations.Count <= 0) return;

            var dbFile = ConnectionString.Split(';').Select(s => s.Split('=')).First(s => s[0].Equals("Data Source"))[1];
            File.Copy(dbFile, dbFile.Replace(".sqlite", $"_backup_{DateTime.UtcNow:yyyy.MM.ddTHH.mm.ss}.sqlite"));
            context.Database.Migrate();

            if (pendingMigrations.Contains("20171211080845_NewPaymentSystem"))
            {
                // Migrate data
                var payments = context.WeekUserLinks.ToList()
                    .GroupBy(l => l.UserId)
                    .Select(g => new Payment
                    {
                        UserId = g.Key,
                        Amount = g.Sum(l => (decimal) l.Paid),
                    })
                    .ToList();

                context.Payments.AddRange(payments);
                context.SaveChanges();
            }
        }

        public static void InitializeAutoMapper()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Week, Week>();
                cfg.CreateMap<WeekUserLink, WeekUserLink>();
                cfg.CreateMap<User, User>();
            });
        }
    }
}
