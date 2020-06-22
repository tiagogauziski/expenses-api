using System;

namespace Expenses.Infrastructure.EventBus.MessageQueue
{
    public interface IMQConsumer
    {
        void Start(string queueName);

        void Stop();

        event EventHandler<MessageReceivedEventArgs> MessagedReceived;
    }
}
