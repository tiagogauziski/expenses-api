using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Queries.Invoice
{
    public class GetInvoiceListQuery
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
