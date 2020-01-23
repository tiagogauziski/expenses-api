using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Application.Statement.ViewModel
{
    public class CreateStatementRequest
    {
        public DateTime Date { get; set; }

        public double Value { get; set; }

        public string Notes { get; set; }

        public Guid InvoiceId { get; set; }
    }
}
