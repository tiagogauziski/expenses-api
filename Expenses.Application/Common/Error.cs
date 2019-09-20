using Expenses.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Application.Common
{
    public class Error
    {
        public Error(DomainValidationEvent validation)
        {
            Message = validation.ValidationMessage;
            ErrorCode = validation.ErrorCode;
        }

        public string ErrorCode { get; set; }

        public string Message { get; set; }
    }
}
