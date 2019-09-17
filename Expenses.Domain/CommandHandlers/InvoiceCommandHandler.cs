using AutoMapper;
using Expenses.Domain.Commands;
using Expenses.Domain.Core.Bus;
using Expenses.Domain.Core.Commands;
using Expenses.Domain.Events;
using Expenses.Domain.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Domain.CommandHandlers
{
    public class InvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, bool>
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IMapper _mapper;

        public InvoiceCommandHandler(IMediatorHandler mediatorHandler,
            IMapper mapper)
        {
            _mediatorHandler = mediatorHandler;
            _mapper = mapper;
        }

        public Task<bool> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                _mediatorHandler.RaiseEvent(new DomainValidationEvent(request.ValidationResult.ToString()));
                return Task.FromResult(false);
            }

            var model = _mapper.Map<Invoice>(request);

            _mediatorHandler.RaiseEvent(new InvoiceCreatedEvent()
            {
                New = model
            });

            return Task.FromResult(true);
        }
    }
}
