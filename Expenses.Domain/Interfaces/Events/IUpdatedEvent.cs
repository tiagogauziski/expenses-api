using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Interfaces.Events
{
    public interface IUpdatedEvent<IModel>
    {
        IModel New { get; set; }

        IModel Old { get; set; }
    }
}
