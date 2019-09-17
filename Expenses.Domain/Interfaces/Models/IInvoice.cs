using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Interfaces.Models
{
    public interface IInvoice
    {
        Guid Id { get; set; }

        string Name { get; set; }

        string Description { get; set; }
    }
}
