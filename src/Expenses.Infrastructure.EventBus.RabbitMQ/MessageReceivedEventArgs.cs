using Expenses.Domain.Core.Events;
using System;

namespace Expenses.Infrastructure.EventBus.RabbitMQ
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(Message message) 
            : base()
        {
            Message = message;
        }

        public Message Message { get; private set; }
    }
}
