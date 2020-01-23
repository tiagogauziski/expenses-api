using Expenses.Domain.Commands.Statement;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Expenses.UnitTests.Domain.Commands.Statement
{
    public class DeleteStatementCommandTests
    {
        private DeleteStatementCommand _command;

        public DeleteStatementCommandTests()
        {
            _command = new DeleteStatementCommand();
            _command.Id = Guid.NewGuid();
        }

        [Fact]
        public void IsValid_Empty_Guid()
        {
            // Arrange
            _command.Id = Guid.Empty;

            // Act
            var result = _command.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_Success()
        {
            // Arrange

            // Act
            var result = _command.IsValid();

            // Assert
            Assert.True(result);
        }
    }
}
