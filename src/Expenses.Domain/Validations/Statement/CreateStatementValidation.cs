using Expenses.Domain.Commands.Statement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Validations.Statement
{
    public class CreateStatementValidation : StatementValidation<CreateStatementCommand>
    {
        public CreateStatementValidation()
        {
            ValidateDate();
            ValidateNotes();
            ValidateValue();
            ValidateInvoiceId();
        }
    }
}
