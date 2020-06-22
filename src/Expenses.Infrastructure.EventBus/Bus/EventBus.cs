using Expenses.Domain.Commands;
using Expenses.Domain.Events;
using Expenses.Infrastructure.EventBus.Events;
using MediatR;
using System.Threading.Tasks;

namespace Expenses.Infrastructure.EventBus.Bus
{
    public sealed class EventBus : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventStore _eventStore;

        public EventBus(
            IMediator mediator,
            IEventStore eventStore)
        {
            _mediator = mediator;
            _eventStore = eventStore;
        }

        public Task<bool> SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public Task RaiseEvent<T>(T @event) where T : Event
        {
            _eventStore.Save(@event);

            return _mediator.Publish(@event);
        }
    }
}
