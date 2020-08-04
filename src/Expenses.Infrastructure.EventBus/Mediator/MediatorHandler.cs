using Expenses.Domain.Commands;
using Expenses.Domain.Events;
using MediatR;
using System.Threading.Tasks;

namespace Expenses.Infrastructure.EventBus.Mediator
{
    public sealed class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventBus _eventStore;

        public MediatorHandler(
            IMediator mediator,
            IEventBus eventStore)
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
