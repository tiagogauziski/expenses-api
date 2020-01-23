using Expenses.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Events
{
    public class DomainValidationEvent : Event
    {
        public DomainValidationEvent(string validationMessage, string errorCode = null)
        {
            ValidationMessage = validationMessage;
            ErrorCode = errorCode;
        }
        public string ValidationMessage { get; private set; }

        public string ErrorCode { get; private set; }
    }
}
