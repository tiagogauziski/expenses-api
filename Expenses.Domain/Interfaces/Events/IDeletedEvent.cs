using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Interfaces.Events
{
    public interface IDeletedEvent<IModel>
    {
        IModel Old { get; set; }
    }
}
