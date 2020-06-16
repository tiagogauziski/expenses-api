using Expenses.API.ViewModel;
using Expenses.Application.Services.Invoice;
using Expenses.Application.Services.Invoice.ViewModel;
using Expenses.Domain.Core.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Expenses.API.Controllers
{
    /// <summary>
    /// Controller to handle Invoice operations
    /// </summary>
    [Route("invoice")]
    [ApiController]
    [Authorize]
    public class InvoiceController : ApiController
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(
            IEventStore eventStore,
            IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        /// <summary>
        /// Creates an invoice
        /// </summary>
        /// <param name="model">Invoice Model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(SuccessfulResponse<InvoiceResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status409Conflict)]
        [Authorize("create:invoices")]
        public async Task<IActionResult> Post([FromBody]CreateInvoiceRequest model)
        {
            var result = await _invoiceService.Create(model);
            if (result.Successful)
                return SuccessResponse(result);
            else
                return FailureResponse(result);
        }

        /// <summary>
        /// Update an invoice
        /// </summary>
        /// <param name="invoiceId">Invoice ID</param>
        /// <param name="model">Invoice Model</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{invoiceId}")]
        [ProducesResponseType(typeof(SuccessfulResponse<InvoiceResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status404NotFound)]
        [Authorize("update:invoices")]
        public async Task<IActionResult> Put([FromRoute] string invoiceId, [FromBody]UpdateInvoiceRequest model)
        {
            model.Id = new Guid(invoiceId);
            var result = await _invoiceService.Update(model);
            if (result.Successful)
                return SuccessResponse(result);
            else
                return FailureResponse(result);
        }

        /// <summary>
        /// Get Invoice by ID
        /// </summary>
        /// <param name="invoiceId">Invoice ID</param>
        /// <returns>Invoice Model</returns>
        [HttpGet]
        [Route("{invoiceId}")]
        [ProducesResponseType(typeof(SuccessfulResponse<InvoiceResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status404NotFound)]
        [Authorize("read:invoices")]
        public async Task<IActionResult> GetById([FromRoute] string invoiceId)
        {
            var result = await _invoiceService.GetById(invoiceId);
            if (result.Successful)
                return SuccessResponse(result);
            else
                return FailureResponse(result);
        }

        /// <summary>
        /// Get Invoice List
        /// </summary>
        /// <param name="query">Get Invoice List Query Parameters</param>
        /// <returns>Invoice Model</returns>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(SuccessfulResponse<IEnumerable<InvoiceResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status404NotFound)]
        [Authorize("read:invoices")]
        public async Task<IActionResult> GetList([FromQuery] GetInvoiceListRequest query)
        {
            var result = await _invoiceService.GetList(query);
            if (result.Successful)
                return SuccessResponse(result);
            else
                return FailureResponse(result);
        }

        /// <summary>
        /// Delete Invoice
        /// </summary>
        /// <param name="invoiceId">Invoice ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{invoiceId}")]
        [ProducesResponseType(typeof(SuccessfulResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailureResponse), StatusCodes.Status404NotFound)]
        [Authorize("delete:invoices")]
        public async Task<IActionResult> Delete([FromRoute] string invoiceId)
        {
            var result = await _invoiceService.Delete(invoiceId);
            if (result.Successful)
                return SuccessResponse(result);
            else
                return FailureResponse(result);
        }
    }
}