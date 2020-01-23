using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Events
{
    public class NotFoundEvent : DomainValidationEvent
    {
        public NotFoundEvent(Guid id, string module, string validationMessage, string errorCode = null) 
            : base(validationMessage, errorCode)
        {
            Id = id;
            Module = module;
        }

        public Guid Id { get; set; }

        public string Module { get; set; }

    }
}
