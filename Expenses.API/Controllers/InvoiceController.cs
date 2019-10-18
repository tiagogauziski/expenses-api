using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expenses.API.ViewModel;
using Expenses.Application.Invoice;
using Expenses.Application.Invoice.ViewModel;
using Expenses.Domain.Core.Events;
using Expenses.Domain.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.API.Controllers
{
    /// <summary>
    /// Controller to handle Invoice operations
    /// </summary>
    [Route("invoice")]
    [ApiController]
    public class InvoiceController : ApiController
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IEventStore eventStore,
            IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(SuccessfulResponse<InvoiceResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Post([FromBody]CreateInvoiceRequest model)
        {
            var result = await _invoiceService.Create(model);
            if (result.Successful)
                return SuccessResponse(result);
            else
                return FailureResponse(result);
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessfulResponse<InvoiceResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromRoute] string id, [FromBody]UpdateInvoiceRequest model)
        {
            model.Id = new Guid(id);
            var result = await _invoiceService.Update(model);
            if (result.Successful)
                return SuccessResponse(result);
            else
                return FailureResponse(result);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessfulResponse<InvoiceResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var result = await _invoiceService.GetById(id);
            if (result.Successful)
                return SuccessResponse(result);
            else
                return FailureResponse(result);
        }
    }
}