using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Expenses.Domain.Models
{
    /// <summary>
    /// Statement model.
    /// </summary>
    public class Statement : ICloneable
    {
        /// <summary>
        /// Gets or sets statement id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets amount.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Gets or sets notes.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the invoice id.
        /// </summary>
        public Guid InvoiceId { get; set; }

        /// <summary>
        /// Gets or sets associated invoice
        /// </summary>
        public Invoice Invoice { get; set; }

        /// <summary>
        /// Gets or sets is paid flag.
        /// </summary>
        public bool IsPaid { get; set; }

        /// <inheritdoc/>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
