using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.API.Response
{
    public class SuccessResponse<T>
    {
        public T Data { get; set; }
    }
}
