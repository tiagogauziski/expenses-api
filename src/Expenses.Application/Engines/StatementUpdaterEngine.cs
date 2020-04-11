using Microsoft.Extensions.Logging;

namespace Expenses.Application.Engines
{
    /// <summary>
    /// Engine to update statements on invoice changes for the year
    /// </summary>
    public class StatementUpdaterEngine
    {
        private readonly ILogger<StatementUpdaterEngine> _logger;

        public StatementUpdaterEngine(
            ILogger<StatementUpdaterEngine> logger)
        {
            _logger = logger;
        }
    }
}
