using Expenses.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Expenses.UnitTests.Domain.Commands
{

    public class UpdateInvoiceCommandTests
    {
        private readonly UpdateInvoiceCommand _command;

        public UpdateInvoiceCommandTests()
        {
            _command = new UpdateInvoiceCommand();
        }

        [Fact]
        public void IsValid_Id_Return_True()
        {
            //arrange
            _command.Id = Guid.NewGuid();
            _command.Name = "Name";
            _command.Description = "Description";

            //act
            var result = _command.IsValid();

            //assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_Id_Return_False()
        {
            //arrange
            _command.Name = "Name";
            _command.Description = "Description";

            //act
            var result = _command.IsValid();

            //assert
            Assert.False(result);
        }
    }
}
