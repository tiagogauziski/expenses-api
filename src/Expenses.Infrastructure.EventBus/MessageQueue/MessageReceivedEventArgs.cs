using System;

namespace Expenses.Infrastructure.EventBus.MessageQueue
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(string message, string routingKey) 
            : base()
        {
            Message = message;
            RoutingKey = routingKey;
        }

        public string Message { get; }
        public string RoutingKey { get; }
    }
}
