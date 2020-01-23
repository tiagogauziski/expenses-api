using Expenses.Domain.Validations.Invoice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Commands.Invoice
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
