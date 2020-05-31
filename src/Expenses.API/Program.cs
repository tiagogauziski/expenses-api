using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using System.Diagnostics;

namespace Expenses.API
{
    public class Program
    {

        public static void Main(string[] args)
        {
            //Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            //Activity.ForceDefaultIdFormat = true;
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration));

    }
}
