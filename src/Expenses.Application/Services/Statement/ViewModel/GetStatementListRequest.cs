using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Application.Services.Statement.ViewModel
{
    /// <summary>
    /// Get Statement List Request Parameters
    /// </summary>
    public class GetStatementListRequest
    {
        /// <summary>
        /// Invoice Id List Filter
        /// </summary>
        public IEnumerable<Guid> InvoiceIdList { get; set; }

        /// <summary>
        /// Start Date Filter
        /// </summary>
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// End Date Filter
        /// </summary>
        public DateTime DateTo { get; set; }
    }
}
