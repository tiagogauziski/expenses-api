using Expenses.Domain.Core.Commands;
using Expenses.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Commands
{
    public abstract class InvoiceCommand : Command
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Recurrence? Recurrence { get; set; }
    }
}
