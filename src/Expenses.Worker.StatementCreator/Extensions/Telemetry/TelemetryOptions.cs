namespace Expenses.Worker.StatementCreator.Extensions.Telemetry
{
    internal class TelemetryOptions
    {
        /// <summary>
        /// Gets or sets whether telemetry is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets jaeger configuration for telemetry.
        /// </summary>
        public JaegerOptions Jaeger { get; set; }

        /// <summary>
        /// Gets or sets azure application insights configuration for telemtry.
        /// </summary>
        public ApplicationInsightsOptions ApplicationInsights { get; set; }
    }
}
