using Expenses.Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.UnitTests.Infrastructure.Bus
{
    public class MockCommand : Command
    {
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
