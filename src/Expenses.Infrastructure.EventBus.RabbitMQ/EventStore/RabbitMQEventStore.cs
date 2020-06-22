﻿using Expenses.Domain.Events;
using Expenses.Infrastructure.EventBus.Events;
using Expenses.Infrastructure.EventBus.MessageQueue;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expenses.Infrastructure.EventBus.RabbitMQ.EventStore
{
    /// <summary>
    /// RabbitMQ implementation of <see cref="IEventStore"/>.
    /// </summary>
    public class RabbitMQEventStore : IEventStore
    {
        private readonly IMQClient _rabbitMqClient;
        private List<Event> _events;

        public RabbitMQEventStore(IMQClient rabbitMqClient)
        {
            _rabbitMqClient = rabbitMqClient ?? throw new ArgumentNullException(nameof(rabbitMqClient));
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

            _rabbitMqClient.Send(@event);
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
