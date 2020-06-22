using Expenses.API.ViewModel;
using Expenses.Application.Common;
using Expenses.Domain.Events;
using Expenses.Domain.Interfaces.Events;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Expenses.API.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        protected ApiController()
        {
        }

        protected HttpStatusCode GetStatusCode<TEvent>(TEvent @event) where TEvent : Event
        {
            if (typeof(TEvent).IsAssignableFrom(typeof(ICreatedEvent<>)))
                return HttpStatusCode.Created;
            else if (@event is DomainValidationEvent)
                return HttpStatusCode.BadRequest;

            return HttpStatusCode.OK;
        }

        protected IActionResult SuccessResponse<TData>(Response<TData> response)
        {
            var result = new ObjectResult(new SuccessfulResponse<TData>(response));
            result.StatusCode = (int)response.StatusCode;

            return result;
        }

        protected IActionResult FailureResponse<TData>(Response<TData> response)
        {
            var result = new ObjectResult(new FailureResponse(response.Error?.Message, response.Error?.ErrorCode));
            result.StatusCode = (int)response.StatusCode;

            return result;
        }
    }
}
