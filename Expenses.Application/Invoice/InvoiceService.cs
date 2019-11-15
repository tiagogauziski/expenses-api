using System;
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
using Expenses.Domain.Interfaces.Repositories;
using Expenses.Domain.Queries.Invoice;

namespace Expenses.Application.Invoice
{
    public class InvoiceService : BaseService, IInvoiceService
    {
        private readonly IMediatorHandler mediatorHandler;
        private readonly IMapper mapper;
        private readonly IEventStore eventStore;
        private readonly IInvoiceRepository invoiceRepository;

        public InvoiceService(IMediatorHandler mediatorHandler,
            IMapper mapper,
            IEventStore eventStore,
            IInvoiceRepository invoiceRepository)
        {
            this.mediatorHandler = mediatorHandler;
            this.mapper = mapper;
            this.eventStore = eventStore;
            this.invoiceRepository = invoiceRepository;
        }

        public async Task<Response<InvoiceResponse>> Create(CreateInvoiceRequest request)
        {
            var command = mapper.Map<CreateInvoiceCommand>(request);

            var result = await mediatorHandler.SendCommand(command);

            if (result)
            {
                var invoiceEvent = eventStore.GetEvent<InvoiceCreatedEvent>();

                var data = mapper.Map<Expenses.Domain.Models.Invoice, InvoiceResponse>(invoiceEvent.New);

                return SuccessfulResponse(data, invoiceEvent);
            }
            else
            {
                var validationEvent = eventStore.GetEvent<DomainValidationEvent>();

                return FailureResponse<InvoiceResponse>(validationEvent);
            }
        }

        public async Task<Response<bool>> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guid))
                return FailureResponse<bool>(new Error("Invalid Guid"), System.Net.HttpStatusCode.BadRequest);

            var command = new DeleteInvoiceCommand();
            command.Id = guid;

            var result = await mediatorHandler.SendCommand(command);

            if (result)
            {
                var invoiceEvent = eventStore.GetEvent<InvoiceDeletedEvent>();

                return SuccessfulResponse(true, invoiceEvent);
            }
            else
            {
                var validationEvent = eventStore.GetEvent<DomainValidationEvent>();

                return FailureResponse<bool>(validationEvent);
            }
        }

        public async Task<Response<InvoiceResponse>> GetById(string id)
        {
            if (!Guid.TryParse(id, out Guid guid))
                return FailureResponse<InvoiceResponse>(new Error("Invalid Guid"), System.Net.HttpStatusCode.BadRequest);

            var result = invoiceRepository.GetById(guid);

            if (result == null)
                return FailureResponse<InvoiceResponse>(new Error("Invoice not found"), System.Net.HttpStatusCode.NotFound);

            var data = mapper.Map<Expenses.Domain.Models.Invoice, InvoiceResponse>(result);

            return SuccessfulResponse(data);
        }

        public async Task<Response<List<InvoiceResponse>>> GetList(GetInvoiceListRequest request)
        {
            var query = mapper.Map<GetInvoiceListQuery>(request);

            var result = invoiceRepository.GetList(query);

            var data = mapper.Map<List<Expenses.Domain.Models.Invoice>, List<InvoiceResponse>>(result);

            return SuccessfulResponse(data);
        }

        public async Task<Response<InvoiceResponse>> Update(UpdateInvoiceRequest request)
        {
            var command = mapper.Map<UpdateInvoiceCommand>(request);

            var result = await mediatorHandler.SendCommand(command);

            if (result)
            {
                var invoiceEvent = eventStore.GetEvent<InvoiceUpdatedEvent>();

                var data = mapper.Map<Expenses.Domain.Models.Invoice, InvoiceResponse>(invoiceEvent.New);

                return SuccessfulResponse(data, invoiceEvent);
            }
            else
            {
                var validationEvent = eventStore.GetEvent<DomainValidationEvent>();

                return FailureResponse<InvoiceResponse>(validationEvent);
            }
        }
    }
}
