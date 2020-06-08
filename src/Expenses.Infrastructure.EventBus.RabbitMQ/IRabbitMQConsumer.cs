using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Infrastructure.EventBus.RabbitMQ
{
    public interface IRabbitMQConsumer : IRabbitMQClient
    {
        void Start(string queueName);

        void Stop();

        event EventHandler<MessageReceivedEventArgs> MessagedReceived;
    }
}
