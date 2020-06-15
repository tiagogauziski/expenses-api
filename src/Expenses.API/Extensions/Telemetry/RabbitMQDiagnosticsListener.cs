using OpenTelemetry.Adapter;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.API.Extensions.Telemetry
{
    public class RabbitMQDiagnosticsListener : ListenerHandler
    {
        public RabbitMQDiagnosticsListener(string sourceName, Tracer tracer) : base(sourceName, tracer)
        {
        }

        public override void OnStartActivity(Activity activity, object payload)
        {
            var span = this.Tracer.StartSpanFromActivity(activity.OperationName, activity, OpenTelemetry.Trace.SpanKind.Client, null);
            foreach (var kv in activity.Tags)
                span.SetAttribute(kv.Key, kv.Value);
        }

        public override void OnStopActivity(Activity activity, object payload)
        {
            var span = this.Tracer.CurrentSpan;
            span.End();
            if (span is IDisposable disposableSpan)
            {
                disposableSpan.Dispose();
            }
        }
    }
}
