using Expenses.Domain.Core.Commands;
using Expenses.Domain.Validations.Statement;
using System;

namespace Expenses.Domain.Commands.Statement
{
    /// <summary>
    /// Delete statements by invoice command 
    /// </summary>
    public class DeleteStatementByInvoiceIdCommand : Command
    {
        public Guid InvoiceId { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new DeleteStatementByInvoiceIdValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
