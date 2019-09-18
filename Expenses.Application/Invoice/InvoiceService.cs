﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Expenses.Application.Common;
using Expenses.Application.Invoice.ViewModel;
using Expenses.Domain.Commands;
using Expenses.Domain.Core.Bus;
using Expenses.Domain.Core.Events;
using Expenses.Domain.Events;
using Expenses.Domain.Interfaces.Models;

namespace Expenses.Application.Invoice
{
    public class InvoiceService : BaseService, IInvoiceService
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IMapper _mapper;
        private readonly IEventStore _eventStore;

        public InvoiceService(IMediatorHandler mediatorHandler,
            IMapper mapper,
            IEventStore eventStore)
        {
            _mediatorHandler = mediatorHandler;
            _mapper = mapper;
            _eventStore = eventStore;
        }

        public async Task<Response<InvoiceResponse>> Create(CreateInvoiceRequest request)
        {
            var createCommand = _mapper.Map<CreateInvoiceCommand>(request);

            var result = await _mediatorHandler.SendCommand(createCommand);

            if (result)
            {
                var invoiceEvent = _eventStore.GetEvent<InvoiceCreatedEvent>();

                var data = _mapper.Map<Expenses.Domain.Models.Invoice, InvoiceResponse>((Domain.Models.Invoice)invoiceEvent.New);

                return SuccessfulResponse(data, invoiceEvent);
            }
            else
            {
                var validationEvent = _eventStore.GetEvent<DomainValidationEvent>();

                return FailureResponse<InvoiceResponse>(validationEvent);
            }
        }
    }
}
