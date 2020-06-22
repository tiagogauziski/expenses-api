using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Expenses.Infrastructure.EventBus.RabbitMQ.Telemetry
{
    public static class ActivityExtensions
    {
        /// <summary>
        /// Extracts activity from RabbitMQ message
        /// </summary>
        /// <param name="source"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Activity ExtractActivity(this BasicDeliverEventArgs source, string name)
        {
            var activity = new Activity(name ?? Constants.RabbitMQMessageActivityName);

            if (source.BasicProperties.Headers.TryGetValue(TraceParent.HeaderKey, out var rawTraceParent) && rawTraceParent is byte[] binRawTraceParent)
            {
                activity.SetParentId(Encoding.UTF8.GetString(binRawTraceParent));
            }

            return activity;
        }
    }
}
