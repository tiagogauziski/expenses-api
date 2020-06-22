using Expenses.Domain.Interfaces.Events;
using Newtonsoft.Json;

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
