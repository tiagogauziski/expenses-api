using Expenses.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.API.ViewModel
{
    public class SuccessfulResponse<TData>
    {
        public SuccessfulResponse(Response<TData> response)
        {
            Data = response.Data;
        }

        public TData Data { get; set; }
    }
}
