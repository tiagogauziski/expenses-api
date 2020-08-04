using Expenses.Infrastructure.EventBus;
using Expenses.Infrastructure.EventBus.InMemory;
using Expenses.UnitTests.Infrastructure.EventBus.InMemory.Bus;
using Moq.AutoMock;
using Xunit;

namespace Expenses.UnitTests.Infrastructure.EventBus.InMemory.EventStore
{
    public class InMemoryEventStoreTests
    {
        private AutoMocker _mocker;
        private IEventBus _eventStore;

        public InMemoryEventStoreTests()
        {
            _mocker = new AutoMocker();

            _eventStore = _mocker.CreateInstance<InMemoryEventBus>();
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
