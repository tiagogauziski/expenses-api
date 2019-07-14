using Expenses.Domain.Commands;
using Expenses.UnitTests.TestExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Expenses.UnitTests.Domain.Commands
{
    public class CreateInvoiceCommandTests
    {
        private CreateInvoiceCommand _command { get; set; }
        public CreateInvoiceCommandTests()
        {

            _command = new CreateInvoiceCommand();
        }

        [Theory]
        [InlineData("Name", "Description")]
        [InlineData("Name", "")]
        [InlineData("Name", null)]
        public void IsValid_ShouldReturnTrue(string name, string description)
        {
            //arrange
            _command.Name = name;
            _command.Description = description;

            //act
            var result = _command.IsValid();

            //assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("", "Description")]
        [InlineData(null, "Description")]
        [InlineData(null, null)]
        public void IsValid_ReturnValidation(string name, string description)
        {
            //arrange
            _command.Name = name;
            _command.Description = description;

            //act
            var result = _command.IsValid();

            //assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_Name_MaxLength_Return_True()
        {
            //arrange
            _command.Name = _command.Name.RandomString(151);

            //act
            var result = _command.IsValid();

            //assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_Description_MaxLength_Return_True()
        {
            //arrange
            _command.Name = _command.Name.RandomString(501);

            //act
            var result = _command.IsValid();

            //assert
            Assert.False(result);
        }
    }
}
