using Expenses.Infrastructure.SqlServer;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace Expenses.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                // This might not be suitable for a production use, as it might not scale well,
                // due to potential race conditions to execute migrations.
                // For production use, make sure to have this in a separate pre-deployment process
                .MigrateDatabase<ExpensesContext>() 
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration));
    }   

    /// <summary>
    /// Extension method to enhance the WebHostBuilder
    /// </summary>
    internal static class ExtensionMethods
    {
        public static IWebHost MigrateDatabase<T>(this IWebHost webHost) where T : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogInformation("Executing database migrations...");

                    var db = services.GetRequiredService<T>();
                    db.Database.Migrate();

                    logger.LogInformation("Database migrations executed successfully.");
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
            return webHost;
        }
    }
}
