using Expenses.Domain.Commands;
using Expenses.Domain.Events;
using Expenses.Infrastructure.EventBus.Events;
using MediatR;
using System.Threading.Tasks;

namespace Expenses.Infrastructure.EventBus.InMemory.Bus
{
    /// <summary>
    /// In Memory implementation of IMediatorHandler.
    /// Responsible for mediating command and event 
    /// </summary>
    public sealed class InMemoryBus : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventStore _eventStore;

        public InMemoryBus(IMediator mediator,
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
