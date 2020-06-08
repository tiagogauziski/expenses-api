namespace Expenses.Worker.StatementCreator.Extensions.Telemetry
{
    internal class JaegerOptions
    {
        /// <summary>
        /// Gets or sets whether jaeger telemetry gathering is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets jaeger service name (manadatory)
        /// </summary>
        public string ServiceName { get; set; }
    }
}
