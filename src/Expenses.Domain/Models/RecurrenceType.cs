using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Expenses.Domain.Models
{
    public enum RecurrenceType
    {
        None,
        Weekly,
        Monthly,
        Yearly,
        Custom
    }
}
