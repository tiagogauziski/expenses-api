using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;

namespace Expenses.Infra.EntityCore
{
    public class ExpensesContextFactory : IDesignTimeDbContextFactory<ExpensesContext>
    {
        public ExpensesContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ExpensesContext>();

            // Get environment
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Build config
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            return new ExpensesContext(optionsBuilder.Options, configuration);
        }
    }
}
