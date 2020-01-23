using Expenses.Domain.Validations.Statement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Commands.Statement
{
    public class UpdateStatementCommand : StatementCommand
    {
        public Guid Id { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new UpdateStatementValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
