using Expenses.Domain.Commands;
using System;

namespace Expenses.UnitTests.Infrastructure.EventBus.InMemory.Bus
{
    public class MockCommand : Command
    {
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
