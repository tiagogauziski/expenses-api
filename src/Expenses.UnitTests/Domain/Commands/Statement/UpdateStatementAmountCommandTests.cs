using Expenses.Domain.Commands.Statement;
using System;
using Xunit;

namespace Expenses.UnitTests.Domain.Commands.Statement
{
    public class UpdateStatementAmountCommandTests
    {
        private UpdateStatementAmountCommand _command;

        public UpdateStatementAmountCommandTests()
        {
            _command = new UpdateStatementAmountCommand();
        }

        private UpdateStatementAmountCommand GetValidCommand()
        {
            var command = new UpdateStatementAmountCommand();
            command.Amount = 123;
            command.IsPaid = true;
            command.Id = Guid.NewGuid();

            return command;
        }

        [Fact]
        public void IsValid_Valid_Id()
        {
            // Arrange
            _command = GetValidCommand();

            // Act
            var result = _command.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_Invalid_Amount()
        {
            // Arrange
            _command = GetValidCommand();
            _command.Amount = -1;

            // Act
            var result = _command.IsValid();

            // Assert
            Assert.False(result);
        }
    }
}
