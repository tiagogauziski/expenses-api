using Expenses.Domain.Validations;
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
            ValidationResult = new UpdateInvoiceValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
