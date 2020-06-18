using Expenses.Domain.Interfaces.Events;
using Newtonsoft.Json;

namespace Expenses.Domain.Events.Invoice
{
    public class InvoiceCreatedEvent : Event, ICreatedEvent<Models.Invoice>
    {
        public Models.Invoice New { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
