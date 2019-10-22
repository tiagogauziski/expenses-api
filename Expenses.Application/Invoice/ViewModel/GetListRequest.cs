using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Application.Invoice.ViewModel
{
    /// <summary>
    /// Search Invoice Parameters
    /// </summary>
    public class GetListRequest
    {
        /// <summary>
        /// Search by Invoice Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Search by Invoice Description
        /// </summary>
        public string Description { get; set; }
    }
}
