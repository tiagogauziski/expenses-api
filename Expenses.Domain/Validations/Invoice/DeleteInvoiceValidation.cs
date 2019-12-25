using Expenses.Domain.Commands.Invoice;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Validations.Invoice
{
    public class DeleteInvoiceValidation : InvoiceValidation<DeleteInvoiceCommand>
    {
        public DeleteInvoiceValidation()
        {
            ValidateId();
        }

        protected void ValidateId()
        {
            RuleFor(i => i.Id)
                .NotEqual(Guid.Empty);
        }
    }
}
