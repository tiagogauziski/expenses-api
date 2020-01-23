using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Interfaces.Events
{
    public interface ICreatedEvent<IModel>
    {
        IModel New { get; set; }
    }
}
