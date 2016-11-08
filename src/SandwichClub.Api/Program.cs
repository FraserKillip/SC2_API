using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace SandwichClub.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(System.IO.Directory.GetCurrentDirectory());

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: true)
                .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                .Build();

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(config)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
