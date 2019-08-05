using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expenses.Application.Invoice;
using Expenses.Application.Invoice.ViewModel;
using Expenses.Domain.Core.Events;
using Expenses.Domain.Events;
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
            IInvoiceService invoiceService) : base(eventStore)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost]
        [Route("")]
        //[ProducesResponseType(typeof(JsonDiffStatusViewModel), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody]InvoiceRequest model)
        {
            var result = await _invoiceService.Create(model);
            if (result != null)
                return SuccessResponse(result);
            else
                return FailureResponse();
        }
    }
}