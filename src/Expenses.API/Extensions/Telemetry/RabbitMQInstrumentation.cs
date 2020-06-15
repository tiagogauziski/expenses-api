using OpenTelemetry.Adapter;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.API.Extensions.Telemetry
{
    /// <summary>
    /// SqlClient instrumentation.
    /// </summary>
    public class RabbitMQInstrumentation : IDisposable
    {
        internal const string RabbitMQDiagnosticListenerName = "RabbitMQDiagnosticListener";

        private readonly DiagnosticSourceSubscriber diagnosticSourceSubscriber;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMQInstrumentation"/> class.
        /// </summary>
        public RabbitMQInstrumentation(Tracer tracer)
        {
            this.diagnosticSourceSubscriber = new DiagnosticSourceSubscriber(
               name => new RabbitMQDiagnosticsListener(name, tracer),
               listener => listener.Name == RabbitMQDiagnosticListenerName,
               null);
            this.diagnosticSourceSubscriber.Subscribe();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.diagnosticSourceSubscriber?.Dispose();
        }
    }
}
