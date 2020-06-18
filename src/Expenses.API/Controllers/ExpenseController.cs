using Microsoft.AspNetCore.Mvc;

namespace Expenses.API.Controllers
{
    /// <summary>
    /// Expenses operations
    /// </summary>
    [Route("expense")]
    [ApiController]
    public class ExpenseController : ApiController
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