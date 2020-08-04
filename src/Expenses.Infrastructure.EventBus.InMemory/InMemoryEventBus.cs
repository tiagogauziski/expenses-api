using Expenses.Domain.Events;
using System.Collections.Generic;
using System.Linq;

namespace Expenses.Infrastructure.EventBus.InMemory
{
    /// <summary>
    /// In Memory implementation of <see cref="IEventBus"/>.
    /// </summary>
    public class InMemoryEventBus : IEventBus
    {
        private List<Event> _events;

        public InMemoryEventBus()
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
            return _events.FirstOrDefault(x => x is TEvent) as TEvent;
        }
    }
}
