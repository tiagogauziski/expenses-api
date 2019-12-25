using Expenses.Domain.Commands.Invoice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Validations.Invoice
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
