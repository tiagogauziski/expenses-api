using Expenses.Domain.Events;
using Expenses.Domain.Events;
using Expenses.Domain.Interfaces.Events;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Response<TData> SuccessfulResponse<TData>(TData data)
        {
            return new Response<TData>(data, HttpStatusCode.OK);
        }

        public Response<TData> FailureResponse<TData>(DomainValidationEvent @event)
        {
            return new Response<TData>(new Error(@event), GetStatusCode(@event));
        }

        public Response<TData> FailureResponse<TData>(Error error, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new Response<TData>(error, statusCode);
        }

        public HttpStatusCode GetStatusCode<TEvent>(TEvent @event) where TEvent : Event
        {
            if (typeof(TEvent).GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICreatedEvent<>)))
                return HttpStatusCode.Created;
            else if (typeof(TEvent).GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IUpdatedEvent<>)))
                return HttpStatusCode.OK;
            else if (typeof(TEvent).GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDeletedEvent<>)))
                return HttpStatusCode.OK;
            else if (@event is NotFoundEvent)
                return HttpStatusCode.NotFound;
            else if (@event is DuplicatedRecordEvent)
                return HttpStatusCode.Conflict;
            else if (@event is DomainValidationEvent)
                return HttpStatusCode.BadRequest;

            return HttpStatusCode.OK;
        }
    }
}
