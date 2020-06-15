using Expenses.Domain.Core.Events;
using Expenses.Infrastructure.EventBus.RabbitMQ.Telemetry;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Text;
using System.Text.Json;

namespace Expenses.Infrastructure.EventBus.RabbitMQ
{
    public class RabbitMQClient : IRabbitMQConsumer
    {
        private const string EXCHANGE_NAME = "expenses.events";
        private const int CONNECTION_RETRIES = 6;

        private IConnection _connection;
        private IModel _channel;
        private EventingBasicConsumer _rabbitMqConsumer;
        private readonly IConfiguration configuration;
        private readonly ILogger<RabbitMQClient> logger;

        public event EventHandler<MessageReceivedEventArgs> MessagedReceived;

        public RabbitMQClient(IConfiguration configuration, ILogger<RabbitMQClient> logger)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Send<TEvent>(TEvent message) where TEvent : Event
        {
            IModel channel = GetChannel();

            channel.BasicPublish(exchange: EXCHANGE_NAME,
                     routingKey: typeof(TEvent).AssemblyQualifiedName,
                     basicProperties: null,
                     body: SerializeMessage(message));
        }

        public void Start(string queueName)
        {
            var channel = GetChannel();

            channel.QueueDeclare(
                queue: queueName, 
                durable: true, 
                exclusive: false,
                autoDelete: false);
            channel.QueueBind(
                queue: queueName,
                exchange: EXCHANGE_NAME,
                routingKey: "#");

            _rabbitMqConsumer = new EventingBasicConsumer(channel);

            _rabbitMqConsumer.Received += RabbitMqConsumer_Received;
            channel.BasicConsume(queueName, true, _rabbitMqConsumer);
            logger.LogInformation("RabbitMQ consumer initialized successfully.");
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        protected void OnMessagedReceived(MessageReceivedEventArgs e)
        {
            logger.LogDebug($"Message received");
            MessagedReceived?.Invoke(this, e);
        }

        private byte[] SerializeMessage<T>(T message)
        {
            string serializedMessage = JsonSerializer.Serialize<T>(message);

            return Encoding.UTF8.GetBytes(serializedMessage);
        }

        private string DeserializeMessage(ReadOnlyMemory<byte> body, string routingKey)
        {
            var message = Encoding.UTF8.GetString(body.ToArray());

            return message;
        }

        /// <summary>
        /// Creating a connection once for the application
        /// https://www.rabbitmq.com/dotnet-api-guide.html#connection-and-channel-lifspan
        /// </summary>
        /// <returns></returns>
        private IConnection CreateConnection()
        {
            if (_connection != null)
            {
                return _connection;
            }

            ConnectionFactory factory;

            factory = new ConnectionFactory();
            factory.Uri = new Uri(configuration.GetConnectionString("RabbitMQ"));

            //
            Policy
                .Handle<BrokerUnreachableException>()
                .WaitAndRetry(CONNECTION_RETRIES, (retry) =>
                {
                    return TimeSpan.FromSeconds(Math.Pow(2, retry));
                }, (exception, timeSpan) => {
                    logger.LogWarning("Failure to create RabbitMQ connection...retrying in {timeSpan}", timeSpan);
                })
                .Execute(() =>
                {
                    _connection = factory.CreateConnection();
                    logger.LogDebug("RabbitMQ created successfully.");
                });

            return _connection;
        }

        private IModel GetChannel()
        {
            if (_channel != null)
            {
                return _channel;
            }

            var connection = CreateConnection();

            _channel = connection.CreateModel().AsActivityEnabled(_connection.Endpoint.HostName);
            _channel.ExchangeDeclare(
                exchange: EXCHANGE_NAME,
                type: ExchangeType.Topic,
                durable: true);

            return _channel;
        }

        private void RabbitMqConsumer_Received(object sender, BasicDeliverEventArgs e)
        {
            string message = DeserializeMessage(e.Body, e.RoutingKey);

            OnMessagedReceived(new MessageReceivedEventArgs(message, e.RoutingKey));
        }
    }
}
