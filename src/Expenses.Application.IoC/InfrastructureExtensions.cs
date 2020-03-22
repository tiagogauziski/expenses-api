using Expenses.Domain.Core.Bus;
using Expenses.Domain.Core.Events;
using Expenses.Domain.Interfaces.Repositories;
using Expenses.Infrastructure.EventBus;
using Expenses.Infrastructure.EventBus.EventStore;
using Expenses.Infrastructure.SqlServer;
using Expenses.Infrastructure.SqlServer.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Expenses.Application.IoC
{
    /// <summary>
    /// Infrastructure dependencies extension methods
    /// </summary>
    public static class InfrastructureExtensions
    {
        /// <summary>
        /// Infrastructure database access dependencies
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

        /// <summary>
        /// Infrastructure message bus dependencies
        /// </summary>
        /// <param name="services"></param>
        public static void AddInfrastructureMessageBus(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // Infrastructure - Message Bus (Mediator)
            services.AddMediatR(typeof(InfrastructureExtensions));

            // Infrastructure - Mediator and EventStore
            services.AddScoped<IMediatorHandler, InMemoryBus>();
            services.AddScoped<IEventStore, InMemoryEventStore>();
        }
    }
}
