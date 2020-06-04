namespace Expenses.API.Extensions.Telemetry
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
        public string InstrumentationKey { get; set; }
    }
}
