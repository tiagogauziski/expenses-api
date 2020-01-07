using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Expenses.Domain.Models
{
    public class Expense
    {
        public Guid Id { get; set; }

        public double Amount { get; set; }

        public DateTime Date { get; set; }

        public Guid InvoiceId { get; set; }

        public Invoice Invoice { get; set; }

    }
}
