using Expenses.Domain.Commands.Invoice;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Validations.Invoice
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

        protected void ValidateRecurrence()
        {
            RuleFor(i => i.Recurrence)
                 .Custom((r, cc) =>
                 {
                     if (r != null && r.RecurrenceType == Models.RecurrenceType.Custom && r.Times <= 0)
                     {
                         cc.AddFailure(new ValidationFailure(nameof(r.RecurrenceType), "If Recurrent Type set to Custom, it should contain number of times."));
                     }

                     if (r != null && r.RecurrenceType == Models.RecurrenceType.Custom && r.Start == DateTime.MinValue)
                     {
                         cc.AddFailure(new ValidationFailure(nameof(r.RecurrenceType), "If Recurrent Type set to Custom, it should have a valid Start Date."));
                     }
                 });

        }
    }
}
