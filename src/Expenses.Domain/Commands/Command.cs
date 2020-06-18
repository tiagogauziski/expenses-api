using System;
using Expenses.Domain.Events;
using FluentValidation.Results;

namespace Expenses.Domain.Commands
{
    /// <summary>
    /// Abstract class designed to send commands from UI to application
    /// </summary>
    public abstract class Command : Message
    {
        protected Command()
        {
            Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; private set; }

        public ValidationResult ValidationResult { get; set; }

        public abstract bool IsValid();
    }
}
