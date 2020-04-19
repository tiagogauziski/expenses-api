using Expenses.Application.Services.Invoice.ViewModel;
using System;

namespace Expenses.Application.Services.Statement.ViewModel
{
    public class StatementResponse
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public string Notes { get; set; }

        public bool IsPaid { get; set; }

        public Guid InvoiceId { get; set; }

        public InvoiceResponse Invoice { get; set; }
    }
}
