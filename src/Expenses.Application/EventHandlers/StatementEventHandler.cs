using Expenses.Domain.Events.Statement;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Application.EventHandlers
{
    public class StatementEventHandler :
        INotificationHandler<StatementCreatedEvent>,
        INotificationHandler<StatementUpdatedEvent>,
        INotificationHandler<StatementDeletedEvent>
    {
        private readonly ILogger<StatementEventHandler> _logger;

        public StatementEventHandler(ILogger<StatementEventHandler> logger)
        {
            _logger = logger;
        }
        public async Task Handle(StatementCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("StatementCreatedEvent: {statement}", notification);

            return;
        }

        public async Task Handle(StatementUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("StatementUpdatedEvent: {statement}", notification);

            return;
        }

        public async Task Handle(StatementDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("StatementDeletedEvent: {statement}", notification);

            return;
        }
    }
}
