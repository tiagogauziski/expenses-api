using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Commands
{
    public class UpdateInvoiceCommand : InvoiceCommand
    {
        public Guid Id { get; set; }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
