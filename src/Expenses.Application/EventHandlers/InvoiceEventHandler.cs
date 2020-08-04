using Expenses.Domain.Events.Invoice;
using Expenses.Infrastructure.EventBus;
using Expenses.Infrastructure.EventBus.Mediator;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.EventHandlers
{
    public class InvoiceEventHandler : 
        INotificationHandler<InvoiceCreatedEvent>,
        INotificationHandler<InvoiceUpdatedEvent>,
        INotificationHandler<InvoiceDeletedEvent>
    {
        private readonly ILogger<InvoiceEventHandler> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IMediatorHandler _mediatorHandler;

        public InvoiceEventHandler(
            ILogger<InvoiceEventHandler> logger,
            ILoggerFactory loggerFactory,
            IMediatorHandler mediatorHandler)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
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
