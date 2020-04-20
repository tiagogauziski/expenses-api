using Expenses.Domain.Core.Commands;
using Expenses.Domain.Validations.Statement;
using System;

namespace Expenses.Domain.Commands.Statement
{
    /// <summary>
    /// Command designed to update statement IsPaid and Value columns
    /// The goal is to update them separetely to have better control over the action.
    /// </summary>
    public class UpdateStatementAmountCommand : Command
    {
        /// <summary>
        /// Statement Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Statement Amount.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Is Paid.
        /// </summary>
        public bool IsPaid { get; set; }

        /// <inheritdoc/>
        public override bool IsValid()
        {
            ValidationResult = new UpdateStatementAmountValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
