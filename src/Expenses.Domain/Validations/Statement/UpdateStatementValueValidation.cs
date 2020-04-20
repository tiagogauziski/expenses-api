using Expenses.Domain.Commands.Statement;
using FluentValidation;
using System;

namespace Expenses.Domain.Validations.Statement
{
    public class UpdateStatementAmountValidation : AbstractValidator<UpdateStatementAmountCommand>
    {
        public UpdateStatementAmountValidation()
        {
            ValidateId();
            ValidateAmount();
        }

        protected void ValidateId()
        {
            RuleFor(i => i.Id)
                .NotEqual(Guid.Empty);
        }

        protected void ValidateAmount()
        {
            RuleFor(p => p.Amount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("The Statement Amount must be a positive number.");
        }
    }
}
