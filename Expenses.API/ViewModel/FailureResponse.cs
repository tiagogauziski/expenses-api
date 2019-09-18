using Expenses.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.API.ViewModel
{
    public class FailureResponse
    {
        public FailureResponse(string message, string errorCode)
        {
            Message = message;
            Code = errorCode;
        }

        public string Message { get; private set; }

        public string Code { get; private set; }
    }
}
