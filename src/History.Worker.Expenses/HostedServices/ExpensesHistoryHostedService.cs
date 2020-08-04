using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Expenses.Domain.Events;
using Expenses.Infrastructure.EventBus.MessageQueue;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace History.Worker.Expenses.HostedServices
{
    public class ExpensesHistoryHostedService : BackgroundService
    {
        private const string AllEventsRoutingKey = "#";
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger logger;
        private readonly IMQConsumer mqConsumer;

        public ExpensesHistoryHostedService(
            IServiceProvider serviceProvider,
            ILogger<ExpensesHistoryHostedService> logger,
            IMQConsumer mqConsumer)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
            this.mqConsumer = mqConsumer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Starting consuming messages...");
            mqConsumer.MessagedReceived += RabbitMqConsumer_MessagedReceived;
            mqConsumer.Start(nameof(ExpensesHistoryHostedService), AllEventsRoutingKey);

            return Task.CompletedTask;
        }

        private async void RabbitMqConsumer_MessagedReceived(object sender, MessageReceivedEventArgs e)
        {
            logger.LogInformation("Consuming message: {routingKey}", e.RoutingKey);

            Event message = null;
            try
            {
                message = JsonSerializer.Deserialize(e.Message, Type.GetType(e.RoutingKey)) as Event;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failure to deserialize Event message.");
                return;
            }

            logger.LogInformation("Finished History Expenses message consumption!");
        }
    }
}
