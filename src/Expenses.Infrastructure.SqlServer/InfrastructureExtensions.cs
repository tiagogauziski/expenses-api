using System;
using Expenses.Domain.Interfaces.Repositories;
using Expenses.Infrastructure.SqlServer.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Expenses.Infrastructure.SqlServer
{
    public static class InfrastructureExtensions
    {
        /// <summary>
        /// Infrastructure message bus dependencies
        /// </summary>
        /// <param name="services"></param>
        public static void AddInfrastructureDatabase(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // Infrastructure - Repositories
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IStatementRepository, StatementRepository>();

            // Infrastructre - DbContext Configuration
            services.AddDbContext<ExpensesContext>();
        }
    }
}
