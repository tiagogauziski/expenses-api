using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Queries.Statement
{
    public class GetStatementListQuery
    {
        public IEnumerable<Guid> InvoiceIdList { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }
    }
}
