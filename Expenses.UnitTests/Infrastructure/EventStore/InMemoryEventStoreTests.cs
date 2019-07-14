using Expenses.Domain.Core.Events;
using Expenses.Infrastructure.EventStore;
using Expenses.UnitTests.Infrastructure.Bus;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Expenses.UnitTests.Infrastructure.EventStore
{
    public class InMemoryEventStoreTests
    {
        private AutoMocker _mocker;
        private IEventStore _eventStore;

        public InMemoryEventStoreTests()
        {
            _mocker = new AutoMocker();

            _eventStore = _mocker.CreateInstance<InMemoryEventStore>();
        }

        [Fact]
        public void SaveGet_ShouldReturnEvent()
        {
            //arrange
            var mockEvent = new MockEvent();

            //act
            _eventStore.Save(mockEvent);
            var result = _eventStore.GetEvent<MockEvent>();

            //assert
            Assert.Equal(mockEvent.MessageId, result.MessageId);
        }

        [Fact]
        public void Get_ShouldNotReturnEvent()
        {
            //arrange

            //act
            var result = _eventStore.GetEvent<MockEvent>();

            //assert
            Assert.Null(result);
        }
    }
}
