using Expenses.Domain.Events;
using Expenses.Domain.Events.Invoice;
using Expenses.Infrastructure.EventBus.MessageQueue;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Worker.StatementCreator.HostedServices
{
    public class StatementCreatorHostedService : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger logger;
        private readonly IMQConsumer mqConsumer;

        public StatementCreatorHostedService(
            IServiceProvider serviceProvider,
            ILogger<StatementCreatorHostedService> logger,
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
            mqConsumer.Start(nameof(StatementCreatorHostedService), nameof(InvoiceCreatedEvent));

            return Task.CompletedTask;
        }

        private async void RabbitMqConsumer_MessagedReceived(object sender, MessageReceivedEventArgs e)
        {
            logger.LogInformation($"Consuming message: {e.RoutingKey}");

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

            using (var scope = serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                await mediator.Publish(message);
            }
        }
    }
}
