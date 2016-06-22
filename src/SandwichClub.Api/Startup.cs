using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SandwichClub.Api.Controllers.Mapper;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Services;
using SandwichClub.Api.Dto;
using SandwichClub.Api.Middleware;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
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
            string cs = "Data Source=" + System.IO.Directory.GetCurrentDirectory() + "/database.sqlite";

            services.AddDbContext<ScContext>(options => options.UseSqlite(cs));

            services.AddTransient<IAuthorisationService, FacebookAuthorisationService>();
            services.AddTransient<IScSession, ScSession>();

            services.AddScoped<IWeekRepository, WeekRepository>();
            services.AddScoped<IWeekService, WeekService>();
            services.AddScoped<IMapper<Week, WeekDto>, WeekMapper>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMapper<User, UserDto>, UserMapper>();

            services.AddScoped<IWeekUserLinkRepository, WeekUserLinkRepository>();
            services.AddScoped<IWeekUserLinkService, WeekUserLinkService>();
            services.AddScoped<IMapper<WeekUserLink, WeekUserLinkDto>, WeekUserLinkMapper>();

            // Configs
            services.Configure<AuthorizationMiddlewareConfig>(config =>
            {
                config.IgnoreAuth = Convert.ToBoolean(Configuration["IgnoreAuth"] ?? "false");
            });
            services.AddOptions();

            // Add framework services.
            services.AddCors();
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseMiddleware<AuthorizationMiddleware>();

            app.UseMvc();
        }
    }
}
