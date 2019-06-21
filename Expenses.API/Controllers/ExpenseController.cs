using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expenses.API.Controllers
{
    /// <summary>
    /// Expenses operations
    /// </summary>
    [Route("expenses")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        /// <summary>
        /// Initialize ExpenseController
        /// </summary>
        public ExpenseController()
        {

        }

        /// <summary>
        /// Create an expense entry
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        //[ProducesResponseType(typeof(JsonDiffStatusViewModel), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult Post()
        {
            return Ok();
        }
    }
}