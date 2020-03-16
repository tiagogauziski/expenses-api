﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Core.Events
{
    public interface IEventStore
    {
        /// <summary>
        /// Save Event into a datasource
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event"></param>
        void Save<TEvent>(TEvent @event) where TEvent : Event;

        /// <summary>
        /// Returns an event based on it's type
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns>Returns an event or null if not found</returns>
        TEvent GetEvent<TEvent>() where TEvent : Event;
    }
}