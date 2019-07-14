using Expenses.Domain.Core.Events;
using System.Collections.Generic;
using System.Linq;

namespace Expenses.Infrastructure.EventStore
{
    /// <summary>
    /// In Memory implementation of IEventStore
    /// </summary>
    public class InMemoryEventStore : IEventStore
    {
        private List<Event> _events;

        public InMemoryEventStore()
        {
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
        }

        /// <summary>
        /// It gets the event 
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns>The event or null if it not found</returns>
        public TEvent GetEvent<TEvent>() where TEvent : Event
        {
            return _events.FirstOrDefault(x => x.GetType() == typeof(TEvent)) as TEvent;
        }
    }
}
