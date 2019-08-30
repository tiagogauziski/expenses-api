using Expenses.Domain.Commands;
using Expenses.Domain.Core.Bus;
using Expenses.Domain.Core.Commands;
using Expenses.Domain.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Domain.CommandHandlers
{
    public class InvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, bool>
    {
        private readonly IMediatorHandler _mediatorHandler;

        public InvoiceCommandHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        public Task<bool> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                _mediatorHandler.RaiseEvent(new DomainValidationEvent(request.ValidationResult.ToString()));
                return Task.FromResult(false);
            }

            _mediatorHandler.RaiseEvent(new InvoiceCreatedEvent()
            {
                Description = "Testing"
            });

            return Task.FromResult(true);
        }
    }
}
