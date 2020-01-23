using Expenses.Domain.Commands.Statement;
using Expenses.Domain.Core.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Validations.Statement
{
    public class DeleteStatementValidation : AbstractValidator<DeleteStatementCommand> 
    {
        public DeleteStatementValidation()
        {
            ValidateId();
        }

        protected void ValidateId()
        {
            RuleFor(i => i.Id)
                .NotEqual(Guid.Empty);
        }
    }
}
