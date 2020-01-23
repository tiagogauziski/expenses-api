using Expenses.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.API.ViewModel
{
    /// <summary>
    /// Successful response from the application carrying a Data payload 
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class SuccessfulResponse<TData>
    {
        /// <summary>
        /// Empty constructor for JsonConvert.DeserializeObject to work correctly
        /// </summary>
        public SuccessfulResponse()
        {
        }

        public SuccessfulResponse(Response<TData> response)
        {
            Data = response.Data;
        }

        public TData Data { get; set; }
    }
}
