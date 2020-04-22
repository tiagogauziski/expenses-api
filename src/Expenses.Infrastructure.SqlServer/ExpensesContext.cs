using Expenses.Domain.Models;
using Expenses.Infrastructure.SqlServer.Configurations;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;


namespace Expenses.Infrastructure.SqlServer
{
    public class ExpensesContext : DbContext
    {
        private const string AZURE_DATABASE_RESOURCE = "https://database.windows.net/";
        private readonly IConfiguration _configuration;

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Statement> Statements { get; set; }

        public ExpensesContext(DbContextOptions<ExpensesContext> options, IConfiguration configuation)
        : base(options)
        {
            _configuration = configuation;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString =
                    _configuration.GetConnectionString("ExpensesDatabase") ?? throw new ArgumentNullException("ConnectionString");

                var sqlConnection = new SqlConnection(connectionString);

                if (_configuration.GetValue<string>("Environment") == "Production")
                {
                    sqlConnection.AccessToken = new AzureServiceTokenProvider().GetAccessTokenAsync(AZURE_DATABASE_RESOURCE).Result;
                }

                optionsBuilder
                    .UseSqlServer(sqlConnection)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
            base.OnConfiguring(optionsBuilder);


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
            modelBuilder.ApplyConfiguration(new StatementConfiguration());
        }
    }
}
