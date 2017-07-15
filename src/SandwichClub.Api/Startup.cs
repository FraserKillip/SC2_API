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
using Microsoft.ApplicationInsights;
using SandwichClub.Api.GraphQL.Middleware;

namespace SandwichClub.Api
{
    public class Startup
    {
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
            string cs = Configuration.GetConnectionString("Database")
                ?? "Data Source=" + System.IO.Directory.GetCurrentDirectory() + "/database.sqlite";
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

            services.AddScoped<IGraphQLAuthenticationValidator, GraphQLAuthenticationValidator>();

            services.AddScoped<UserType>();
            services.AddScoped<WeekType>();
            services.AddScoped<WeekUserLinkType>();
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
            context.Database.Migrate();


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
