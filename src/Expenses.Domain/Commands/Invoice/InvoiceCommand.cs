using Expenses.Domain.Models;

namespace Expenses.Domain.Commands.Invoice
{
    public abstract class InvoiceCommand : Command
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Recurrence? Recurrence { get; set; }
    }
}
