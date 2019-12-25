using Expenses.Domain.Validations.Invoice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Commands.Invoice
{
    public class DeleteInvoiceCommand : InvoiceCommand
    {
        public Guid Id { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new DeleteInvoiceValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
