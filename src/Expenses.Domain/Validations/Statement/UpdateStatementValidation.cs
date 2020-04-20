using Expenses.Domain.Commands.Statement;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Validations.Statement
{
    public class UpdateStatementValidation : StatementValidation<UpdateStatementCommand>
    {
        public UpdateStatementValidation()
        {
            ValidateId();
            ValidateDate();
            ValidateNotes();
            ValidateAmount();
            ValidateInvoiceId();
        }

        protected void ValidateId()
        {
            RuleFor(i => i.Id)
                .NotEqual(Guid.Empty);
        }
    }
}
