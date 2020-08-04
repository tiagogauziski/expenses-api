using System;
using Expenses.Infrastructure.EventBus.MessageQueue;
using Microsoft.Extensions.DependencyInjection;

namespace Expenses.Infrastructure.EventBus.ServiceBus
{
    public static class InfrastructureExtensions
    {
        /// <summary>
        /// Add Azure Service Bus into dependency injection
        /// </summary>
        /// <param name="services"></param>
        public static void AddAzureMessageBus(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<IEventBus, ServiceBusEventBus>();
            services.AddSingleton<IMQClient, ServiceBusMQClient>();
            services.AddSingleton<IMQConsumer, ServiceBusMQClient>();
        }
    }
}
