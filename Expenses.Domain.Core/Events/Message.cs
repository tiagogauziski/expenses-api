using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Core.Events
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
