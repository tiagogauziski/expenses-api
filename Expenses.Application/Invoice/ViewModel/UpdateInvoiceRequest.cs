using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Application.Invoice.ViewModel
{
    public class UpdateInvoiceRequest : CreateInvoiceRequest
    {
        public Guid Id { get; set; }
    }
}
