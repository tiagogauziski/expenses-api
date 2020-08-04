using System;
using Expenses.Infrastructure.EventBus.Mediator;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Expenses.Infrastructure.EventBus
{
    public static class InfrastructureExtensions
    {
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
            services.AddScoped<IMediatorHandler, MediatorHandler>();

        }
    }
}
