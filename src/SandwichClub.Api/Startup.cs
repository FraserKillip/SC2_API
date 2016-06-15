using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Services;
using SandwichClub.Api.AuthorisationMiddleware;
using SandwichClub.Api.DTO;
using SandwichClub.Api.Repositories.Models;
using SandwichClub.Api.Services.Mapper;

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

            services.AddDbContext<SC2Context>(options => options.UseSqlite(cs));

            services.AddTransient<IWeekRepository, WeekRepository>();
            services.AddTransient<IWeekService, WeekService>();
            services.AddTransient<IMapper<Week, WeekDto>, WeekMapper>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IMapper<User, UserDto>, UserMapper>();

            services.AddTransient<IWeekUserLinkRepository, WeekUserLinkRepository>();
            services.AddTransient<IWeekUserLinkService, WeekUserLinkService>();
            services.AddTransient<IMapper<WeekUserLink, WeekUserLinkDto>, WeekUserLinkMapper>();

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
