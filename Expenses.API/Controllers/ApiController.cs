using Expenses.Domain.Core.Events;
using Expenses.Domain.Events;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.API.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        private IEventStore _eventStore;

        protected ApiController(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        protected IActionResult SuccessResponse<T>(T response)
        {
            return Created("", response);
        }

        protected IActionResult FailureResponse()
        {
            var validation = _eventStore.GetEvent<DomainValidationEvent>();
            return BadRequest(validation);
        }
    }
}
