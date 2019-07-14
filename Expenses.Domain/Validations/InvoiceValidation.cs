using Expenses.Domain.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Validations
{
    public abstract class InvoiceValidation<T> : AbstractValidator<T> where T : InvoiceCommand
    {
        protected void ValidateName()
        {
            RuleFor(i => i.Name)
                .NotEmpty().WithMessage("Please ensure you have entered the Name")
                .Length(0, 150).WithMessage("The Name must have between 0 and 150 characters");
        }

        protected void ValidateDescription()
        {
            RuleFor(i => i.Description)
                .Length(0, 500).WithMessage("The Name must have between 0 and 500 characters");
        }

        protected void ValidateId()
        {
            RuleFor(i => i.Id)
                .NotEqual(Guid.Empty);
        }
    }
}
