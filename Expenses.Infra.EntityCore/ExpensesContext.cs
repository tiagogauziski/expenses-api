using Expenses.Domain.Models;
using Expenses.Infra.EntityCore.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Infra.EntityCore
{
    public class ExpensesContext : DbContext
    {
        public DbSet<Invoice> Invoices { get; set; }

        public ExpensesContext(DbContextOptions<ExpensesContext> options)
        : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseInMemoryDatabase("Expenses")
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
        }
    }
}
