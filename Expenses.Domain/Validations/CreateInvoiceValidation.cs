using Expenses.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Validations
{
    public class CreateInvoiceValidation : InvoiceValidation<InvoiceCommand>
    {
        public CreateInvoiceValidation()
        {
            ValidateName();
            ValidateDescription();
        }
    }
}
