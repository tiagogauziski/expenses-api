namespace Expenses.OpenTelemetry.Options
{
    public class JaegerOptions
    {
        /// <summary>
        /// Gets or sets whether jaeger telemetry gathering is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets jaeger service name (manadatory)
        /// </summary>
        public string ServiceName { get; set; }

        public string AgentHost { get; set; } = "locahost";

        public int AgentPort { get; set; } = 6831;
    }
}
