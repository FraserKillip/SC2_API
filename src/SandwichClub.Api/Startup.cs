using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SandwichClub.Api.Controllers.Mapper;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Services;
using SandwichClub.Api.Dto;
using SandwichClub.Api.Middleware;
using SandwichClub.Api.Repositories.Models;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Linq;
using GraphQL.Middleware;
using GraphQL.Types;
using SandwichClub.Api.GraphQL;
using SandwichClub.Api.GraphQL.Types;
using System;
using System.IO;

namespace SandwichClub.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Console.WriteLine("Env content root path");
            Console.WriteLine(env.ContentRootPath);
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Console.WriteLine($"appsettings.{env.EnvironmentName}.json");
            Console.WriteLine(File.ReadAllText($"{env.ContentRootPath}/appsettings.{env.EnvironmentName}.json"));
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string cs;
            if (Configuration.GetConnectionString("Database") != null) {
                cs = Configuration.GetConnectionString("Database");
            } else {
                cs = "Data Source=" + System.IO.Directory.GetCurrentDirectory() + "/database.sqlite";
            }

            Console.WriteLine(cs);

            services.AddDbContext<ScContext>(options => options.UseSqlite(cs).UseMemoryCache(null));

            InitializeAutoMapper();
            services.AddSingleton<IMapper>(Mapper.Instance);

            services.AddTransient<IAuthorisationService, FacebookAuthorisationService>();
            services.AddScoped<IScSession, ScSession>();

            services.AddScoped<IWeekRepository, WeekRepository>();
            services.AddScoped<IWeekService, WeekService>();
            services.AddScoped<IMapper<Week, WeekDto>, WeekMapper>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMapper<User, UserDto>, UserMapper>();

            services.AddScoped<IWeekUserLinkRepository, WeekUserLinkRepository>();
            services.AddScoped<IWeekUserLinkService, WeekUserLinkService>();
            services.AddScoped<IMapper<WeekUserLink, WeekUserLinkDto>, WeekUserLinkMapper>();

            services.AddScoped<UserType>();
            services.AddScoped<WeekType>();
            services.AddScoped<WeekUserLinkType>();
            services.AddScoped<SandwichClubQuery>();
            services.AddScoped<SandwichClubMutation>();
            services.AddScoped<SandwichClubSchema>((sp) => new SandwichClubSchema(type => (GraphType) sp.GetService(type)));

            // Configs
            services.Configure<AuthorizationMiddlewareConfig>(Configuration);
            services.AddOptions();

            // Add framework services.
            services.AddCors();
            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Migrate the database
            var context = app.ApplicationServices.GetService<ScContext>();
            context.Database.Migrate();


            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseMiddleware<AuthorizationMiddleware>();

            app.UseGraphQL(new GraphQLOptions
            {
                GraphQLPath = "/graphql" ,
            });

            app.UseGraphiQL(new GraphiQLOptions()
            {
                GraphiQLPath = "/graphiql"
            });

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
