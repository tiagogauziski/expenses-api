using Expenses.Domain.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Domain.EventHandlers
{
    public class InvoiceEventHandler : INotificationHandler<InvoiceCreatedEvent>
    {
        public async Task Handle(InvoiceCreatedEvent notification, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();

            return;
        }
    }
}
