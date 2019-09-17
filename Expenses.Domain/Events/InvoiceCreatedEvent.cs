using Expenses.Domain.Core.Events;
using Expenses.Domain.Interfaces.Events;
using Expenses.Domain.Interfaces.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Events
{
    public class InvoiceCreatedEvent : Event, ICreatedEvent<IInvoice>
    {
        public IInvoice New { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
