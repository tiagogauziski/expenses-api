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

        public async Task<Response<InvoiceResponse>> GetById(string id)
        {
            if (!Guid.TryParse(id, out Guid guid))
                return FailureResponse<InvoiceResponse>(new Error("Invalid Guid"), System.Net.HttpStatusCode.BadRequest);

            var result = await invoiceRepository.GetById(guid);

            if (result == null)
                return FailureResponse<InvoiceResponse>(new Error("Invoice not found"), System.Net.HttpStatusCode.NotFound);

            var data = mapper.Map<Expenses.Domain.Models.Invoice, InvoiceResponse>(result);

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
