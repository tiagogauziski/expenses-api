﻿using Expenses.Domain.Interfaces.Events;
using System.Text.Json;

namespace Expenses.Domain.Events.Statement
{
    public class StatementCreatedEvent : Event, ICreatedEvent<Models.Statement>
    {
        public Models.Statement New { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
