using Expenses.Domain.Events;
using Expenses.Infrastructure.EventBus.Events;
using Expenses.Infrastructure.EventBus.MessageQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Expenses.Infrastructure.EventBus.ServiceBus.EventStore
{
    /// <summary>
    /// Azure Service Bus implementation of <see cref="IEventStore"/>.
    /// </summary>
    public class ServiceBusEventStore : IEventStore
    {
        private readonly IMQClient mqClient;

        private List<Event> _events;

        public ServiceBusEventStore(IMQClient mqClient)
        {
            this.mqClient = mqClient ?? throw new ArgumentNullException(nameof(mqClient));

            _events = new List<Event>();
        }

        /// <summary>
        /// Save an event into an datasource
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