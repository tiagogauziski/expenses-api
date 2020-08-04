using Expenses.Infrastructure.EventBus.InMemory;
using Expenses.Infrastructure.SqlServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Expenses.IntegrationTests
{
    public class CustomWebApplicationFactorySqlite<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly string _connectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;

        public CustomWebApplicationFactorySqlite()
        {
            _connection = new SqliteConnection(_connectionString);
            _connection.Open();
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = Host.CreateDefaultBuilder()
                              .ConfigureWebHostDefaults(x =>
                              {
                                  x.UseStartup<TStartup>().UseTestServer();
                              })
                              .ConfigureAppConfiguration((hostingContext, config) =>
                              {
                                  config.AddJsonFile("appsettings.json");
                              });
            return builder;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .ConfigureTestServices(services =>
                {
                    services
                       .AddAuthentication(options =>
                       {
                           options.DefaultAuthenticateScheme = "Test";
                           options.DefaultChallengeScheme = "Test";
                       })
                       .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                })
                .ConfigureServices(services =>
                {
                    // Remove the app's ApplicationDbContext registration.
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                            typeof(DbContextOptions<ExpensesContext>));

                    if (descriptor != null)
                        services.Remove(descriptor);

                    // Replace RabbitMQ bus to InMemoryBus to test it locally.
                    services.AddInMemoryMessageBus();

                    // Add ApplicationDbContext using an in-memory database for testing.
                    services.AddEntityFrameworkSqlite();
                    services.AddDbContext<ExpensesContext>((context) =>
                    {
                        context.UseSqlite(_connection)
                            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    });

                    // Build the service provider.
                    var sp = services.BuildServiceProvider();

                    // Create a scope to obtain a reference to the database
                    // context (ApplicationDbContext).
                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<ExpensesContext>();
                        var logger = scopedServices
                            .GetRequiredService<ILogger<CustomWebApplicationFactorySqlite<TStartup>>>();

                        // Ensure the database is created.
                        db.Database.EnsureCreated();
                        try
                        {
                            // Seed the database with test data.
                            //Utilities.InitializeDbForTests(db);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "An error occurred seeding the " +
                                "database with test messages. Error: {Message}", ex.Message);
                        }
                    }
                });
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _connection.Close();
        }

    }
}
