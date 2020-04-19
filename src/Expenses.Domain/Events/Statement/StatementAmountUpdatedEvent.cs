using Expenses.Domain.Core.Events;
using Expenses.Domain.Interfaces.Events;
using System.Text.Json;

namespace Expenses.Domain.Events.Statement
{
    /// <summary>
    /// Track changes of statement Amount and IsPaid columns
    /// </summary>
    public class StatementAmountUpdatedEvent : Event, IUpdatedEvent<Models.Statement>
    {
        public Models.Statement New { get; set; }
        public Models.Statement Old { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
