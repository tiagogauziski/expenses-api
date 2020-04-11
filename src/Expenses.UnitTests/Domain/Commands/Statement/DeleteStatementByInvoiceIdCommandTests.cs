using Expenses.Domain.Commands.Statement;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Expenses.UnitTests.Domain.Commands.Statement
{
    public class DeleteStatementByInvoiceIdCommandTests
    {
        private DeleteStatementByInvoiceIdCommand _command;

        public DeleteStatementByInvoiceIdCommandTests()
        {
            _command = new DeleteStatementByInvoiceIdCommand();
            _command.InvoiceId = Guid.NewGuid();
        }

        [Fact]
        public void InvoiceId_Empty_Guid()
        {
            // Arrange
            _command.InvoiceId = Guid.Empty;

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
