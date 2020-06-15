using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Infrastructure.EventBus.RabbitMQ.Telemetry
{
    internal class Constants
    {
        public const string DiagnosticsName = "RabbitMQDiagnosticListener";

        public const string ExchangeTagName = "exchange";

        public const string RoutingKeyTagName = "routingKey";

        public const string ApplicationInsightsTelemetryType = "rabbitmq";

        public const string OperationTagName = "operation";

        public const string MessageSizeTagName = "messageSize";

        public const string PublishOperation = "publish";

        public const string PublishActivityName = "Publish to RabbitMQ";

        public const string HostTagName = "host";

        public const string RabbitMQMessageActivityName = "RabbitMQ Message";
    }
}
