using Expenses.Domain.Commands.Statement;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Validations.Statement
{
    public class StatementValidation<T> : AbstractValidator<T> where T : StatementCommand
    {
        protected void ValidateDate()
        {
            RuleFor(i => i.Date)
                .GreaterThan(DateTime.MinValue).WithMessage("The Statement Date must be a valid date.");
        }

        protected void ValidateValue()
        {
            RuleFor(p => p.Value)
                .GreaterThanOrEqualTo(0)
                .WithMessage("The Statement Value must be a positive number.");
        }

        protected void ValidateInvoiceId()
        {
            RuleFor(p => p.InvoiceId)
                .NotEqual(Guid.Empty)
                .WithMessage("The Statement Invoice Id must be a valid Guid."); 
        }

        protected void ValidateNotes()
        {
            RuleFor(i => i.Notes)
                .Length(0, 500).WithMessage("The Name must have between 0 and 500 characters.");
        }
    }
}
