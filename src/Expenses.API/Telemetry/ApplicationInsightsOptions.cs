using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.API.Telemetry
{
    internal class ApplicationInsightsOptions
    {
        /// <summary>
        /// Gets or sets whether azure application insights telemetry gathering is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets application insights application key.
        /// </summary>
        public string ApplicationKey { get; set; }
    }
}
