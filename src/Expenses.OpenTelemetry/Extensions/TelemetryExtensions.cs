using Expenses.OpenTelemetry.Options;
using Expenses.OpenTelemetry.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Configuration;
using OpenTelemetry.Trace.Samplers;
using System.Collections.Generic;
using System.Reflection;

namespace Expenses.OpenTelemetry.Extensions
{
    public static class TelemetryExtensions
    {
        public static string TracerServiceName { get; }

        static TelemetryExtensions()
        {
            TracerServiceName = Assembly.GetEntryAssembly().GetName().Name.ToString();
        }

        public static Tracer GetApplicationTracer(this TracerFactoryBase tracerFactory)
        {
            return tracerFactory.GetTracer(TracerServiceName);
        }

        public static void AddTelemetry(this IServiceCollection services, TelemetryOptions telemetryOptions)
        {
            if (telemetryOptions.Enabled)
            {
                services.AddOpenTelemetry((sp, builder) =>
                {
                    if (telemetryOptions.Jaeger.Enabled)
                    {
                        builder
                            .UseJaeger(options =>
                            {
                                options.ServiceName = TracerServiceName;
                                options.AgentHost = telemetryOptions.Jaeger.AgentHost;
                                options.AgentPort = telemetryOptions.Jaeger.AgentPort;
                            });
                    }
                    else if (telemetryOptions.ApplicationInsights.Enabled)
                    {
                        builder
                            .UseApplicationInsights(options =>
                            {
                                options.InstrumentationKey = telemetryOptions.ApplicationInsights.InstrumentationKey;
                            });
                    }
                    builder
                        .SetSampler(new AlwaysOnSampler())
                        .AddRequestAdapter()
                        .AddDependencyAdapter(config =>
                        {
                            config.SetHttpFlavor = true;
                        })
                        .AddAdapter((tracer) => new RabbitMQInstrumentation(tracer))
                        .SetResource(new Resource(new Dictionary<string, object>
                        {
                            { "service.name", TracerServiceName }
                        }));
                });
            }
        }
    }
}
