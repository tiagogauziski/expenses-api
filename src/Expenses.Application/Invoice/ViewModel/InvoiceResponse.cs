using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Application.Invoice.ViewModel
{
    public class InvoiceResponse
    {
        /// <summary>
        /// Invoice ID
        /// </summary>
        public Guid Id { get; set; }

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
