﻿using Expenses.Domain.Commands.Invoice;
using Expenses.UnitTests.TestExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Expenses.UnitTests.Domain.Commands.Invoice
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

        [Fact]
        public void IsValid_Recurrence_Custom()
        {
            //arrange
            _command.Name = "Name";
            _command.Description = "Description";
            _command.Recurrence = new Expenses.Domain.Models.Recurrence();
            _command.Recurrence.RecurrenceType = Expenses.Domain.Models.RecurrenceType.Custom;

            //act
            var result = _command.IsValid();

            //assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_Recurrence_Custom_Success()
        {
            //arrange
            _command.Name = "Name";
            _command.Description = "Description";
            _command.Recurrence = new Expenses.Domain.Models.Recurrence();
            _command.Recurrence.RecurrenceType = Expenses.Domain.Models.RecurrenceType.Custom;
            _command.Recurrence.Start = DateTime.UtcNow;
            _command.Recurrence.Times = 3;

            //act
            var result = _command.IsValid();

            //assert
            Assert.True(result);
        }
    }
}
