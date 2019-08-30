using Expenses.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Events
{
    public class DomainValidationEvent : Event
    {
        public DomainValidationEvent(string validationMessage)
        {
            ValidationMessage = validationMessage;
        }
        public string ValidationMessage { get; private set; }
    }
}
