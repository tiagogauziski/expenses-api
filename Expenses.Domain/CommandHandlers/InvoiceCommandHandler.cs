using AutoMapper;
using Expenses.Domain.Commands;
using Expenses.Domain.Core.Bus;
using Expenses.Domain.Core.Commands;
using Expenses.Domain.Events;
using Expenses.Domain.Interfaces.Repositories;
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
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceCommandHandler(IMediatorHandler mediatorHandler,
            IMapper mapper,
            IInvoiceRepository invoiceRepository)
        {
            _mediatorHandler = mediatorHandler;
            _mapper = mapper;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<bool> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await _mediatorHandler.RaiseEvent(new DomainValidationEvent(request.ValidationResult.ToString()));
                return false;
            }

            var model = _mapper.Map<Invoice>(request);

            _invoiceRepository.Create(model);

            await _mediatorHandler.RaiseEvent(new InvoiceCreatedEvent()
            {
                New = model
            });

            return true;
        }

        public async Task<bool> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await _mediatorHandler.RaiseEvent(new DomainValidationEvent(request.ValidationResult.ToString()));
                return false;
            }

            var oldInvoice = _invoiceRepository.GetById(request.Id);
            if (oldInvoice == null)
            {
                await _mediatorHandler.RaiseEvent(new NotFoundEvent(request.Id, "Invoice", "Invoice not found"));
                return false;
            }

            var model = _mapper.Map<Invoice>(request);

            _invoiceRepository.Update(model);

            await _mediatorHandler.RaiseEvent(new InvoiceUpdatedEvent()
            {
                Old = oldInvoice,
                New = model
            });

            return true;
        }
    }
}
