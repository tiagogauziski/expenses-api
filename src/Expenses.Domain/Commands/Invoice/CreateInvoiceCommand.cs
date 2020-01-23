using Expenses.Domain.Validations.Invoice;
using System;

namespace Expenses.Domain.Commands.Invoice
{
    public class CreateInvoiceCommand : InvoiceCommand
    {
        public override bool IsValid()
        {
            ValidationResult = new CreateInvoiceValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
