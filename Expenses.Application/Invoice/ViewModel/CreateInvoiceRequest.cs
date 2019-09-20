using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Expenses.Application.Invoice.ViewModel
{
    public class CreateInvoiceRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
