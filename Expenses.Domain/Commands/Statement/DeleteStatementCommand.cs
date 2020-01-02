using Expenses.Domain.Core.Commands;
using Expenses.Domain.Validations.Statement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Commands.Statement
{
    public class DeleteStatementCommand : Command
    {
        public Guid Id { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new DeleteStatementValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
