using Expenses.Domain.Core.Events;
using System.Collections.Generic;
using System.Text.Json;

namespace Expenses.Domain.Events.Statement
{
    public class StatementBulkDeletedEvent : Event
    {
        public IReadOnlyList<Models.Statement> Old { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
