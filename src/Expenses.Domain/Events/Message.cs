using MediatR;
using System;

namespace Expenses.Domain.Events
{
    public abstract class Message : IRequest<bool>
    {
        public Guid MessageId { get; set; }

        protected Message()
        {
            MessageId = Guid.NewGuid();
        }
    }
}
