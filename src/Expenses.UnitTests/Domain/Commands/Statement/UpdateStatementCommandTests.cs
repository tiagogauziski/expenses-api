using Expenses.Domain.Commands.Statement;
using Expenses.UnitTests.TestExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Expenses.UnitTests.Domain.Commands.Statement
{
    public class UpdateStatementCommandTests
    {
        private UpdateStatementCommand _command;

        public UpdateStatementCommandTests()
        {
            _command = new UpdateStatementCommand();
        }

        private UpdateStatementCommand GetValidCommand()
        {
            var command = new UpdateStatementCommand();
            command.Id = Guid.NewGuid();
            command.Date = DateTime.UtcNow;
            command.Notes.RandomString(500);
            command.Value = 123;
            command.InvoiceId = Guid.NewGuid();

            return command;
        }

        [Fact]
        public void IsValid_Id_Empty()
        {
            // Arrange
            _command = GetValidCommand();
            _command.Id = Guid.Empty;

            // Act
            var result = _command.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_Date_MinValue()
        {
            // Arrange
            _command = GetValidCommand();
            _command.Date = DateTime.MinValue;

            // Act
            var result = _command.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_Notes_MaxValue()
        {
            // Arrange
            _command = GetValidCommand();
            _command.Notes = _command.Notes.RandomString(501);

            // Act
            var result = _command.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_Notes_Null()
        {
            // Arrange
            _command = GetValidCommand();
            _command.Notes = null;

            // Act
            var result = _command.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_Value_MinValue()
        {
            // Arrange
            _command = GetValidCommand();
            _command.Value = -1;

            // Act
            var result = _command.IsValid();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_InvoiceId_Empty()
        {
            // Arrange
            _command = GetValidCommand();
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
            _command = GetValidCommand();
            _command.InvoiceId = Guid.Empty;

            // Act
            var result = _command.IsValid();

            // Assert
            Assert.False(result);
        }
    }
}
