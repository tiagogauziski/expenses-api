using Expenses.Application.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.API.ViewModel
{
    public class FailureResponse
    {
        public FailureResponse()
        {

        }

        public FailureResponse(string message, string errorCode)
        {
            Message = message;
            Code = errorCode;
        }

        public string Message { get; set; }

        public string Code { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
