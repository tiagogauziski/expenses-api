using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Application.Services.Statement.ViewModel
{
    public class CreateStatementRequest
    {
        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public string Notes { get; set; }

        public bool IsPaid { get; set; }

        public Guid InvoiceId { get; set; }
    }
}
