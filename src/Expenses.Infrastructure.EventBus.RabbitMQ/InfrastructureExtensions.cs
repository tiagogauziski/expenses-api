using System;
using Expenses.Infrastructure.EventBus.MessageQueue;
using Microsoft.Extensions.DependencyInjection;

namespace Expenses.Infrastructure.EventBus.RabbitMQ
{
    public static class InfrastructureExtensions
    {
        /// <summary>
        /// Infrastructure message bus dependencies
        /// </summary>
        /// <param name="services"></param>
        public static void AddRabbitMQMessageBus(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<IEventBus, RabbitMQEventBus>();
            services.AddSingleton<IMQClient, RabbitMQClient>();
            services.AddSingleton<IMQConsumer, RabbitMQClient>();
        }
    }
}
