using Expenses.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Expenses.Application.Invoice.ViewModel
{
    /// <summary>
    /// Invoice Recurrence
    /// </summary>
    public class InvoiceRecurrence
    {
        /// <summary>
        /// Recurrence type
        /// </summary>
        public RecurrenceType RecurrenceType { get; set; }

        /// <summary>
        /// Start Week/Day/Month/Year
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// If Custom recurrence, how many times per year.
        /// </summary>
        public int Times { get; set; }
    }
}
