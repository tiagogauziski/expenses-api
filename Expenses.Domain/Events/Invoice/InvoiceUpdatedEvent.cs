using Expenses.Domain.Core.Events;
using Expenses.Domain.Interfaces.Events;
using Expenses.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Events.Invoice
{
    public class InvoiceUpdatedEvent : Event, IUpdatedEvent<Models.Invoice>
    {
        public Models.Invoice New { get; set; }
        public Models.Invoice Old { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
