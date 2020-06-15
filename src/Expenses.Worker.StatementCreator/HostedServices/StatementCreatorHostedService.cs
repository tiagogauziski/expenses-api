using Expenses.Domain.Core.Events;
using Expenses.Infrastructure.EventBus.RabbitMQ;
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
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        private readonly IRabbitMQConsumer _rabbitMqConsumer;

        public StatementCreatorHostedService(
            IServiceProvider serviceProvider,
            ILogger<StatementCreatorHostedService> logger,
            IRabbitMQConsumer rabbitMqConsumer
            )
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _rabbitMqConsumer = rabbitMqConsumer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting consuming messages...");
            _rabbitMqConsumer.MessagedReceived += RabbitMqConsumer_MessagedReceived;
            _rabbitMqConsumer.Start(nameof(StatementCreatorHostedService));

            return Task.CompletedTask;
        }

        private async void RabbitMqConsumer_MessagedReceived(object sender, MessageReceivedEventArgs e)
        {
            _logger.LogInformation($"Consuming message: {e.RoutingKey}");

            Event message = null;
            try
            {
                message = JsonSerializer.Deserialize(e.Message, Type.GetType(e.RoutingKey)) as Event;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failure to deserialize Event message.");
                return;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                await mediator.Publish(message);
            }
        }
    }
}
