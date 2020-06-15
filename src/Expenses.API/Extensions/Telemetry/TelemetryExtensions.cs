using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace.Configuration;
using OpenTelemetry.Trace.Samplers;
using System;
using System.Diagnostics;

namespace Expenses.API.Extensions.Telemetry
{
    public static class TelemetryExtensions
    {
        internal static void AddTelemetry(this IServiceCollection services, TelemetryOptions telemetryOptions)
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
                                options.ServiceName = telemetryOptions.Jaeger.ServiceName;
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
                        .AddAdapter((tracer) => new RabbitMQInstrumentation(tracer));
                });
            }
        }
    }
}
