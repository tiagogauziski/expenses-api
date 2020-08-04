using System;
using Microsoft.Extensions.DependencyInjection;

namespace Expenses.Infrastructure.EventBus.InMemory
{
    public static class InfrastructureExtensions
    {
        /// <summary>
        /// Add in-memory message bus into dependency injection.
        /// </summary>
        /// <param name="services"></param>
        public static void AddInMemoryMessageBus(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<IEventBus, InMemoryEventBus>();
        }
    }
}
