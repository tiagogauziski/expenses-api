﻿using Expenses.Domain.Core.Bus;
using Expenses.Domain.Core.Commands;
using Expenses.Domain.Core.Events;
using MediatR;
using System.Threading.Tasks;

namespace Expenses.Infrastructure.EventBus.RabbitMQ.Bus
{
    /// <summary>
    /// In Memory implementation of IMediatorHandler.
    /// Responsible for mediating command and event 
    /// </summary>
    public sealed class RabbitMQBus : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventStore _eventStore;

        public RabbitMQBus(
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
