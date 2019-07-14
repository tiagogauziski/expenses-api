using Expenses.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Events
{
    public class InvoiceCreatedEvent : Event
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

    }
}
