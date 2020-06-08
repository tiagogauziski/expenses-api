using Expenses.Infrastructure.EventBus.RabbitMQ;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
            _rabbitMqConsumer.MessagedReceived += RabbitMqConsumer_MessagedReceived;
            _rabbitMqConsumer.Start(nameof(StatementCreatorHostedService));


            return Task.CompletedTask;
        }

        private async void RabbitMqConsumer_MessagedReceived(object sender, MessageReceivedEventArgs e)
        {
            _logger.LogInformation($"Consuming message: {e.Message}");

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator =
                    scope.ServiceProvider
                        .GetRequiredService<IMediator>();

                await mediator.Publish(e.Message);
            }
        }
    }
}
