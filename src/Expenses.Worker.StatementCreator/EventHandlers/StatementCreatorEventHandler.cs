using System;
using System.Threading;
using System.Threading.Tasks;
using Expenses.Application.Engines;
using Expenses.Application.EventHandlers;
using Expenses.Domain.Events.Invoice;
using Expenses.Infrastructure.EventBus.Mediator;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Expenses.Worker.StatementCreator.EventHandlers
{
    public class StatementCreatorEventHandler :
        INotificationHandler<InvoiceCreatedEvent>
    {
        private readonly ILogger<InvoiceEventHandler> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IMediatorHandler _mediatorHandler;

        public StatementCreatorEventHandler(
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

            var statementCreatorEngine = new StatementCreatorEngine(_loggerFactory.CreateLogger<StatementCreatorEngine>());

            var statementList = statementCreatorEngine.Run(notification.New, DateTime.Now);

            foreach (var statement in statementList)
            {
                await _mediatorHandler.SendCommand(statement);
            }

            return;
        }
    }
}
