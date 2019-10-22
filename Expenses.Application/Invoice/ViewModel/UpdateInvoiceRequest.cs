using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Application.Invoice.ViewModel
{
    public class UpdateInvoiceRequest : CreateInvoiceRequest
    {
        /// <summary>
        /// Invoice ID
        /// </summary>
        public Guid Id { get; set; }
    }
}
