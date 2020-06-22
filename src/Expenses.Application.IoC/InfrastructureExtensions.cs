using Expenses.Domain.Interfaces.Repositories;
using Expenses.Infrastructure.EventBus;
using Expenses.Infrastructure.EventBus.Bus;
using Expenses.Infrastructure.EventBus.Events;
using Expenses.Infrastructure.EventBus.InMemory.EventStore;
using Expenses.Infrastructure.EventBus.MessageQueue;
using Expenses.Infrastructure.EventBus.RabbitMQ;
using Expenses.Infrastructure.EventBus.RabbitMQ.EventStore;
using Expenses.Infrastructure.EventBus.ServiceBus;
using Expenses.Infrastructure.EventBus.ServiceBus.EventStore;
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

            // Infrastructure - Event Bus
            services.AddScoped<IMediatorHandler, EventBus>();

            // Infrastructure - Mediator and EventStore
            if (IsAzure())
            {
                // Infrastructure - Azure ServiceBus implementation 
                services.AddAzureServiceBus();
            }
            else
            {
                // Infrastructure - RabbitMQ implementation 
                services.AddRabbitMQBus();
            }
            
        }

        public static void AddRabbitMQBus(this IServiceCollection services)
        {
            services.AddScoped<IEventStore, RabbitMQEventStore>();
            services.AddSingleton<IMQClient, RabbitMQClient>();
            services.AddSingleton<IMQConsumer, RabbitMQClient>();
        }

        private static void AddAzureServiceBus(this IServiceCollection services)
        {
            services.AddScoped<IEventStore, ServiceBusEventStore>();
            services.AddSingleton<IMQClient, ServiceBusMQClient>();
            services.AddSingleton<IMQConsumer, ServiceBusMQClient>();
        }

        public static void AddInMemoryBus(this IServiceCollection services)
        {
            services.AddScoped<IEventStore, InMemoryEventStore>();
        }

        private static bool IsAzure()
        {
            return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));
        }
    }
}
