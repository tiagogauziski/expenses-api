using Expenses.Application.Engines;
using Expenses.Domain.Core.Bus;
using Expenses.Domain.Events.Invoice;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
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
            _logger = logger;
            _loggerFactory = loggerFactory;
            _mediatorHandler = mediatorHandler;
        }
        public async Task Handle(InvoiceCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("InvoiceCreatedEvent: {invoice}", notification);

            var statementCreatorEngine = new StatementCreatorEngine(_loggerFactory.CreateLogger<StatementCreatorEngine>());

            var statementList = statementCreatorEngine.Run(notification.New, DateTime.Now);

            foreach (var statement in statementList)
            {
                await _mediatorHandler.SendCommand(statement);
            }

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
