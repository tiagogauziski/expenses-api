using Expenses.Domain.Interfaces.Events;
using Newtonsoft.Json;

namespace Expenses.Domain.Events.Invoice
{
    public class InvoiceDeletedEvent : Event, IDeletedEvent<Models.Invoice>
    {
        public Models.Invoice Old { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
