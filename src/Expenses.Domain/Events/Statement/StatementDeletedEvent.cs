using Expenses.Domain.Core.Events;
using Expenses.Domain.Interfaces.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Events.Statement
{
    public class StatementDeletedEvent : Event, IDeletedEvent<Models.Statement>
    {
        public Models.Statement Old { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
