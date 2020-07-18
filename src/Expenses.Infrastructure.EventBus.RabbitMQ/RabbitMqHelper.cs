using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace Expenses.Infrastructure.EventBus.RabbitMQ
{
    internal static class RabbitMqHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Source: https://github.com/rabbitmq/rabbitmq-dotnet-client/issues/415
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="properties"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static string GetCustomProperty<T>(this IBasicProperties properties, string key)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (properties.Headers.ContainsKey(key))
            {
                var value = properties.Headers[key];

                // HACK: We set this as a string, but deliveries will return a UTF8-encoded byte[].
                if (value.GetType() == typeof(byte[]))
                {
                    return Encoding.UTF8.GetString((byte[])value);
                }
                else
                {
                    throw new InvalidCastException($"{value.GetType()} is an unknown property value type");
                }
            }

            return null;
        }
    }
}
