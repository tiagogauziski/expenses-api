using Expenses.Domain.Events;
using Expenses.Infrastructure.EventBus.MessageQueue;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expenses.Infrastructure.EventBus.RabbitMQ
{
    /// <summary>
    /// RabbitMQ implementation of <see cref="IEventBus"/>.
    /// </summary>
    public class RabbitMQEventBus : IEventBus
    {
        private readonly IMQClient mqClient;

        private List<Event> _events;

        public RabbitMQEventBus(IMQClient mqClient)
        {
            this.mqClient = mqClient ?? throw new ArgumentNullException(nameof(mqClient));
            _events = new List<Event>();
        }

        /// <summary>
        /// Save an event into an in-memory datasource
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        public void Save<T>(T @event) where T : Event
        {
            _events.Add(@event);

            mqClient.Send(@event);
        }

        /// <summary>
        /// It gets the event 
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns>The event or null if it not found</returns>
        public TEvent GetEvent<TEvent>() where TEvent : Event
        {
            return _events.FirstOrDefault(x => x is TEvent) as TEvent;
        }
    }
}
