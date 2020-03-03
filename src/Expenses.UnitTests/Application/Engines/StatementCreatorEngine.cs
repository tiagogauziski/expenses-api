using Expenses.Application.Engines;
using Expenses.Domain.Models;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Expenses.UnitTests.Application.Engines
{
    public class StatementCreatorEngineTests
    {
        private readonly AutoMocker _mocker;
        private readonly StatementCreatorEngine _engine;

        public StatementCreatorEngineTests()
        {
            _mocker = new AutoMocker();

            _engine = _mocker.CreateInstance<StatementCreatorEngine>();
        }

        [Fact]
        public async void Run_Recurrence_Null_Validation()
        {
            // Arrange
            var invoice = new Invoice()
            {
                Recurrence = null
            };

            // Act
            var result = _engine.Run(invoice, new DateTime(DateTime.Now.Year, 1, 1));

            // Assess
            Assert.Empty(result);
        }

        [Fact]
        public async void Run_GenerateWeekly()
        {
            // Arrange
            var invoice = new Invoice()
            {
                Recurrence = new Recurrence()
                {
                    RecurrenceType = RecurrenceType.Weekly,
                    Start = new DateTime(2020, 1, 1)
                }
            };

            // Act
            var result = _engine.Run(invoice, new DateTime(2020, 1, 1));

            // Assess
            Assert.NotEmpty(result);
            Assert.Equal(53, result.Count);
        }

        [Fact]
        public async void Run_GenerateMonthly()
        {
            // Arrange
            var invoice = new Invoice()
            {
                Recurrence = new Recurrence()
                {
                    RecurrenceType = RecurrenceType.Monthly,
                    Start = DateTime.Now
                }
            };

            // Act
            var result = _engine.Run(invoice, new DateTime(DateTime.Now.Year, 1, 1));

            // Assess
            Assert.NotEmpty(result);
            Assert.Equal(12, result.Count);
        }

        [Fact]
        public async void Run_GenerateYearly()
        {
            // Arrange
            var invoice = new Invoice()
            {
                Recurrence = new Recurrence()
                {
                    RecurrenceType = RecurrenceType.Yearly,
                    Start = DateTime.Now
                }
            };

            // Act
            var result = _engine.Run(invoice, new DateTime(DateTime.Now.Year, 1, 1));

            // Assess
            Assert.NotEmpty(result);
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public async void Run_GenerateCustomMonthly()
        {
            // Arrange
            var invoice = new Invoice()
            {
                Recurrence = new Recurrence()
                {
                    RecurrenceType = RecurrenceType.Custom,
                    Start = new DateTime(DateTime.Now.Year, 1, 1),
                    Times = 12
                }
            };

            // Act
            var result = _engine.Run(invoice, new DateTime(DateTime.Now.Year, 1, 1));

            // Assess
            Assert.NotEmpty(result);
            Assert.Equal(12, result.Count);
        }
    }
}
