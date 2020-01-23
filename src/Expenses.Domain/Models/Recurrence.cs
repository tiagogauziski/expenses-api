using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Models
{
    public class Recurrence
    {
        public RecurrenceType RecurrenceType { get; set; }

        public DateTime Start { get; set; }

        public int Times { get; set; }
    }
}
