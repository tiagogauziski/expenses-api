using Expenses.Domain.Models;
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
            optionsBuilder.UseInMemoryDatabase("Expenses").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }
}
