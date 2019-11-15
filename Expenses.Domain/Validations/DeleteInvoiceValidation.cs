using Expenses.Domain.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Validations
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
