using Expenses.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Domain.EventHandlers
{
    public class InvoiceEventHandler : 
        INotificationHandler<InvoiceCreatedEvent>,
        INotificationHandler<InvoiceUpdatedEvent>,
        INotificationHandler<InvoiceDeletedEvent>
    {
        private readonly ILogger<InvoiceEventHandler> _logger;

        public InvoiceEventHandler(ILogger<InvoiceEventHandler> logger)
        {
            _logger = logger;
        }
        public async Task Handle(InvoiceCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("InvoiceCreatedEvent: {invoice}", notification);

            return;
        }

        public async Task Handle(InvoiceUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("InvoiceUpdatedEvent: {invoice}", notification);

            return;
        }

        public async Task Handle(InvoiceDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("InvoiceDeletedEvent: {invoice}", notification);

            return;
        }
    }
}
