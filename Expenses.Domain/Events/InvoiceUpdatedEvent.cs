using Expenses.Domain.Core.Events;
using Expenses.Domain.Interfaces.Events;
using Expenses.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Events
{
    public class InvoiceUpdatedEvent : Event, IUpdatedEvent<Invoice>
    {
        public Invoice New { get; set; }
        public Invoice Old { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
