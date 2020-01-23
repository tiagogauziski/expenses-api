using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Models
{
    public class Statement
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public double Value { get; set; }

        public string Notes { get; set; }

        public Guid InvoiceId { get; set; }

        public Invoice Invoice { get; set; }
    }
}
