using Expenses.Domain.Models;
using Expenses.Infrastructure.SqlServer.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;


namespace Expenses.Infrastructure.SqlServer
{
    public class ExpensesContext : DbContext
    {
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

                optionsBuilder
                    .UseSqlServer(connectionString)
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
