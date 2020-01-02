using Expenses.Domain.Validations.Statement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Commands.Statement
{
    public class CreateStatementCommand : StatementCommand
    {
        public override bool IsValid()
        {
            ValidationResult = new CreateStatementValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
