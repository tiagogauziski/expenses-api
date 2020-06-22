using System;

namespace Expenses.Domain.Commands.Statement
{
    public abstract class StatementCommand : Command
    {
        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public string Notes { get; set; }

        public bool IsPaid { get; set; }

        public Guid InvoiceId { get; set; }
    }
}
