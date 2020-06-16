using Expenses.Domain.Core.Events;
using Expenses.Infrastructure.EventBus;
using Expenses.Infrastructure.EventBus.InMemory.Bus;
using MediatR;
using Moq;
using Moq.AutoMock;
using System.Threading;
using Xunit;

namespace Expenses.UnitTests.Infrastructure.EventBus.InMemory.Bus
{
    public class InMemoryBusTests
    {
        private AutoMocker _mocker;
        private IMediatorHandler _inMemoryBus;

        public InMemoryBusTests()
        {
            _mocker = new AutoMocker();

            _inMemoryBus = _mocker.CreateInstance<InMemoryBus>();
        }

        [Fact]
        public void SendCommand_CallMediator()
        {
            //arrange
            _mocker.GetMock<IMediator>()
                .Setup(m => m.Send(It.IsAny<IRequest<bool>>(), It.IsAny<CancellationToken>()))
                .Verifiable();
            var command = new MockCommand();

            //act
            var result = _inMemoryBus.SendCommand(command);

            //assert
            _mocker.GetMock<IMediator>().Verify();
        }

        [Fact]
        public void Publish_CallMediator()
        {
            //arrange
            _mocker.GetMock<IMediator>()
                .Setup(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()))
                .Verifiable();

            _mocker.GetMock<IEventStore>()
                .Setup(m => m.Save(It.IsAny<Event>()))
                .Verifiable();

            var @event = new MockEvent();

            //act

            var result = _inMemoryBus.RaiseEvent(@event);

            //assert
            _mocker.GetMock<IMediator>().Verify();
            _mocker.GetMock<IEventStore>().Verify();
        }
    }
}
