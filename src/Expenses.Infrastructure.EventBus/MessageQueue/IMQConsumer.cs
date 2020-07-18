using System;

namespace Expenses.Infrastructure.EventBus.MessageQueue
{
    public interface IMQConsumer
    {
        void Start(string queueName, string routingKey);

        void Stop();

        event EventHandler<MessageReceivedEventArgs> MessagedReceived;
    }
}
