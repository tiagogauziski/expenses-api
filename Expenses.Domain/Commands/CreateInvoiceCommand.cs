using Expenses.Domain.Validations;
using System;

namespace Expenses.Domain.Commands
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
