﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Expenses.Application.Common;
using Expenses.Application.Services.Invoice.ViewModel;
using Expenses.Domain.Commands.Invoice;
using Expenses.Domain.Commands.Statement;
using Expenses.Domain.Events;
using Expenses.Domain.Events.Invoice;
using Expenses.Domain.Interfaces.Repositories;
using Expenses.Domain.Queries.Invoice;
using Expenses.Infrastructure.EventBus;
using Expenses.Infrastructure.EventBus.Mediator;

namespace Expenses.Application.Services.Invoice
{
    public class InvoiceService : BaseService, IInvoiceService
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IMapper _mapper;
        private readonly IEventBus _eventStore;
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(
            IMediatorHandler mediatorHandler,
            IMapper mapper,
            IEventBus eventStore,
            IInvoiceRepository invoiceRepository)
        {
            _mediatorHandler = mediatorHandler;
            _mapper = mapper;
            _eventStore = eventStore;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<Response<InvoiceResponse>> Create(CreateInvoiceRequest request)
        {
            var command = _mapper.Map<CreateInvoiceCommand>(request);

            var result = await _mediatorHandler.SendCommand(command);

            if (result)
            {
                var invoiceEvent = _eventStore.GetEvent<InvoiceCreatedEvent>();

                var data = _mapper.Map<Expenses.Domain.Models.Invoice, InvoiceResponse>(invoiceEvent.New);

                return SuccessfulResponse(data, invoiceEvent);
            }
            else
            {
                var validationEvent = _eventStore.GetEvent<DomainValidationEvent>();

                return FailureResponse<InvoiceResponse>(validationEvent);
            }
        }

        public async Task<Response<bool>> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guid))
                return FailureResponse<bool>(new Error("Invalid Guid"), System.Net.HttpStatusCode.BadRequest);

            var invoiceCommand = new DeleteInvoiceCommand() { Id = guid };
            var statementCommand = new DeleteStatementByInvoiceIdCommand() { InvoiceId = guid };

            if (!await _mediatorHandler.SendCommand(statementCommand))
            {
                var validationEvent = _eventStore.GetEvent<DomainValidationEvent>();

                return FailureResponse<bool>(validationEvent);
            }

            if (await _mediatorHandler.SendCommand(invoiceCommand))
            {
                var invoiceEvent = _eventStore.GetEvent<InvoiceDeletedEvent>();

                return SuccessfulResponse(true, invoiceEvent);
            }
            else
            {
                var validationEvent = _eventStore.GetEvent<DomainValidationEvent>();

                return FailureResponse<bool>(validationEvent);
            }
        }

        public async Task<Response<InvoiceResponse>> GetById(string id)
        {
            if (!Guid.TryParse(id, out Guid guid))
                return FailureResponse<InvoiceResponse>(new Error("Invalid Guid"), System.Net.HttpStatusCode.BadRequest);

            var result = _invoiceRepository.GetById(guid);

            if (result == null)
                return FailureResponse<InvoiceResponse>(new Error("Invoice not found"), System.Net.HttpStatusCode.NotFound);

            var data = _mapper.Map<Expenses.Domain.Models.Invoice, InvoiceResponse>(result);

            return SuccessfulResponse(data);
        }

        public async Task<Response<List<InvoiceResponse>>> GetList(GetInvoiceListRequest request)
        {
            var query = _mapper.Map<GetInvoiceListQuery>(request);

            var result = _invoiceRepository.GetList(query);

            var data = _mapper.Map<List<Expenses.Domain.Models.Invoice>, List<InvoiceResponse>>(result);

            return SuccessfulResponse(data);
        }

        public async Task<Response<InvoiceResponse>> Update(UpdateInvoiceRequest request)
        {
            var command = _mapper.Map<UpdateInvoiceCommand>(request);

            var result = await _mediatorHandler.SendCommand(command);

            if (result)
            {
                var invoiceEvent = _eventStore.GetEvent<InvoiceUpdatedEvent>();

                var data = _mapper.Map<Expenses.Domain.Models.Invoice, InvoiceResponse>(invoiceEvent.New);

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
