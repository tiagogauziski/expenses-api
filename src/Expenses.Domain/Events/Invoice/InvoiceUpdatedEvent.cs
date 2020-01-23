using Expenses.Domain.Core.Events;
using Expenses.Domain.Interfaces.Events;
using System.Text.Json;

namespace Expenses.Domain.Events.Invoice
{
    public class InvoiceUpdatedEvent : Event, IUpdatedEvent<Models.Invoice>
    {
        public Models.Invoice New { get; set; }
        public Models.Invoice Old { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
