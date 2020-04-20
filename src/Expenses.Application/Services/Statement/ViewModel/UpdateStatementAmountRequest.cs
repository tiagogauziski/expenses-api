using System;

namespace Expenses.Application.Services.Statement.ViewModel
{
    /// <summary>
    /// Request to update a specific statement Amount and IsPaid columns
    /// </summary>
    public class UpdateStatementAmountRequest
    {
        /// <summary>
        /// Gets or sets Statement Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets Amount paid
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Gets or sets IsPaid flag
        /// </summary>
        public bool IsPaid { get; set; }
    }
}
