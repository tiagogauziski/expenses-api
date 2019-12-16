using Expenses.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Validations
{
    public class CreateInvoiceValidation : InvoiceValidation<CreateInvoiceCommand>
    {
        public CreateInvoiceValidation()
        {
            ValidateName();
            ValidateDescription();
            ValidateRecurrence();
        }
    }
}
