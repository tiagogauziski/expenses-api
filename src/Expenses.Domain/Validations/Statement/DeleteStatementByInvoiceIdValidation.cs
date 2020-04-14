using Expenses.Domain.Commands.Statement;
using FluentValidation;
using System;

namespace Expenses.Domain.Validations.Statement
{
    public class DeleteStatementByInvoiceIdValidation 
        : AbstractValidator<DeleteStatementByInvoiceIdCommand>
    {
        public DeleteStatementByInvoiceIdValidation()
        {
            ValidateInvoiceId();
        }

        protected void ValidateInvoiceId()
        {
            RuleFor(p => p.InvoiceId)
                .NotEqual(Guid.Empty)
                .WithMessage("The Statement Invoice Id must be a valid Guid.");
        }
    }
}
