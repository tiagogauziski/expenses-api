using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Expenses.Application.Services.Invoice.ViewModel
{
    public class CreateInvoiceRequest
    {
        /// <summary>
        /// Invoice Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Invoice Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Invoice Recurrence Details
        /// </summary>
        public InvoiceRecurrence? Recurrence { get; set; }
    }
}
