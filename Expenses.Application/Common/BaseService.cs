﻿using Expenses.Domain.Core.Events;
using Expenses.Domain.Events;
using Expenses.Domain.Interfaces.Events;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Expenses.Application.Common
{
    public abstract class BaseService
    {
        public BaseService()
        {

        }

        public Response<TData> SuccessfulResponse<TData, TEvent>(TData data, TEvent @event) where TEvent : Event
        {
            return new Response<TData>(data, GetStatusCode(@event));
        }

        public Response<TData> FailureResponse<TData>(DomainValidationEvent @event)
        {
            return new Response<TData>(new Error(@event), GetStatusCode(@event));
        }

        public HttpStatusCode GetStatusCode<TEvent>(TEvent @event) where TEvent : Event
        {
            if (typeof(TEvent).IsAssignableFrom(typeof(ICreatedEvent<>)))
                return HttpStatusCode.Created;
            else if (@event is DomainValidationEvent)
                return HttpStatusCode.BadRequest;

            return HttpStatusCode.OK;
        }
    }
}
