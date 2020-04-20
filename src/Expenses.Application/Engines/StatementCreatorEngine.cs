using Expenses.Domain.Commands.Statement;
using Expenses.Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expenses.Application.Engines
{
    /// <summary>
    /// Engine to generate statements for the year.
    /// </summary>
    public class StatementCreatorEngine
    {
        private readonly ILogger<StatementCreatorEngine> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatementCreatorEngine"/> class.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public StatementCreatorEngine(
            ILogger<StatementCreatorEngine> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Run the Statement Creator Engine with custom parameters.
        /// </summary>
        /// <param name="invoice">Invoice.</param>
        /// <param name="referenceDate">Reference Date.</param>
        /// <returns>List of Statements.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IReadOnlyList<StatementCommand> Run(Invoice invoice, DateTime referenceDate)
        {
            if (invoice == null) throw new ArgumentNullException(nameof(invoice));
            
            IReadOnlyList<StatementCommand> generatedStatements = new List<StatementCommand>();

            if (invoice.Recurrence == null)
            {
                _logger.LogWarning("Invoice Recurrence is null. Exiting...");
                return generatedStatements;
            }

            switch (invoice.Recurrence.RecurrenceType)
            {
                case RecurrenceType.Weekly:
                    generatedStatements = GenerateWeekly(invoice, referenceDate);
                    break;
                case RecurrenceType.Monthly:
                    generatedStatements = GenerateMonthly(invoice, referenceDate);
                    break;
                case RecurrenceType.Yearly:
                    generatedStatements = GenerateYearly(invoice, referenceDate);
                    break;
                case RecurrenceType.Custom:
                    generatedStatements = GenerateCustom(invoice, referenceDate);
                    break;
                default:
                    break;
            }

            return generatedStatements.ToList();
        }

        private IReadOnlyList<StatementCommand> GenerateWeekly(Invoice invoice, DateTime referenceDate)
        {
            var result = new List<StatementCommand>();
            var dayOfWeek = invoice.Recurrence.Start.DayOfWeek;
            var date = new DateTime(referenceDate.Year, referenceDate.Month, referenceDate.Day);

            while(dayOfWeek != date.DayOfWeek)
            {
                date = date.AddDays(1);
            }

            while(date.Year == referenceDate.Year)
            {
                result.Add(new CreateStatementCommand()
                {
                    Date = date,
                    InvoiceId = invoice.Id
                });

                date = date.AddDays(7);
            }

            _logger.LogInformation("Generated {statementCount} weekly statement recurrences for Invoice {invoice}", result.Count, invoice);

            return result;
        }

        private IReadOnlyList<StatementCommand> GenerateMonthly(Invoice invoice, DateTime referenceDate)
        {
            var result = new List<StatementCommand>();
            var date = new DateTime(referenceDate.Year, referenceDate.Month, invoice.Recurrence.Start.Day);

            while (date.Year == referenceDate.Year)
            {
                result.Add(new CreateStatementCommand()
                {
                    Date = date,
                    InvoiceId = invoice.Id
                });

                date = date.AddMonths(1);
            }

            _logger.LogInformation("Generated {statementCount} monthly statement recurrences for Invoice {invoice}", result.Count, invoice);

            return result;
        }

        private IReadOnlyList<StatementCommand> GenerateYearly(Invoice invoice, DateTime referenceDate)
        {
            var result = new List<StatementCommand>();
            var date = new DateTime(referenceDate.Year, invoice.Recurrence.Start.Month, invoice.Recurrence.Start.Day);

            result.Add(new CreateStatementCommand()
            {
                Date = date,
                InvoiceId = invoice.Id
            });

            _logger.LogInformation("Generated {statementCount} yearly statement recurrences for Invoice {invoice}", result.Count, invoice);

            return result;
        }

        private IReadOnlyList<StatementCommand> GenerateCustom(Invoice invoice, DateTime referenceDate)
        {
            var result = new List<StatementCommand>();
            var date = new DateTime(referenceDate.Year, referenceDate.Month, invoice.Recurrence.Start.Day);

            var recurrence = 0;
            while (date.Year == referenceDate.Year &&
                recurrence < invoice.Recurrence.Times)
            {
                result.Add(new CreateStatementCommand()
                {
                    Date = date,
                    InvoiceId = invoice.Id
                });

                date = date.AddMonths(1);
                recurrence++;
            }

            _logger.LogInformation("Generated {statementCount} custom monthly statement recurrences for Invoice {invoice}", result.Count, invoice);

            return result;
        }
    }
}
