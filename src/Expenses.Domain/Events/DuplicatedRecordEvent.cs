using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Events
{
    public class DuplicatedRecordEvent : DomainValidationEvent
    {
        public DuplicatedRecordEvent()
            : base(string.Empty, string.Empty)
        {

        }

        public DuplicatedRecordEvent(string field, string module, string validationMessage, string errorCode = null) 
            : base(validationMessage, errorCode)
        {
            Field = field;
            Module = module;
        }

        public string Field { get; set; }

        public string Module { get; set; }

    }
}
