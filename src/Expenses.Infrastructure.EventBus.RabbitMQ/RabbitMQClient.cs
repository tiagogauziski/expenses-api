using Expenses.Domain.Core.Events;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace Expenses.Infrastructure.EventBus.RabbitMQ
{
    public class RabbitMQClient : IRabbitMQClient, IRabbitMQConsumer
    {
        private const string EXCHANGE_NAME = "expenses";

        private IConnection _connection;
        private IModel _consumerChannel;
        private EventingBasicConsumer _rabbitMqConsumer;
        private IConfiguration _configuration;

        public event EventHandler<MessageReceivedEventArgs> MessagedReceived;

        public RabbitMQClient(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void Send<TEvent>(TEvent message) where TEvent : Event
        {
            IModel channel = GetChannel();

            channel.BasicPublish(exchange: EXCHANGE_NAME,
                     routingKey: typeof(TEvent).Name,
                     basicProperties: null,
                     body: SerializeMessage(message));
        }

        public void Start(string queueName)
        {
            if (_consumerChannel is null)
            {
                _consumerChannel = GetChannel();
            }

            _consumerChannel.QueueDeclare(queue: queueName, durable: true, exclusive: false);
            _consumerChannel.QueueBind(queue: queueName,
                              exchange: EXCHANGE_NAME,
                              routingKey: "*");

            _rabbitMqConsumer = new EventingBasicConsumer(_consumerChannel);

            _rabbitMqConsumer.Received += RabbitMqConsumer_Received;
            _consumerChannel.BasicConsume(queueName, true, _rabbitMqConsumer);
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        protected void OnMessagedReceived(MessageReceivedEventArgs e)
        {
            MessagedReceived?.Invoke(this, e);
        }

        private byte[] SerializeMessage<T>(T message)
        {
            string serializedMessage = JsonSerializer.Serialize<T>(message);

            return Encoding.UTF8.GetBytes(serializedMessage);
        }

        private object DeserializeMessage(ReadOnlyMemory<byte> body, string routingKey)
        {
            var message = Encoding.UTF8.GetString(body.ToArray());

            return JsonSerializer.Deserialize(message, Type.GetType(routingKey));
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

            ConnectionFactory _factory;

            _factory = new ConnectionFactory();
            _factory.Uri = new Uri(_configuration.GetConnectionString("RabbitMQ"));

            return _factory.CreateConnection();
        }

        private IModel GetChannel()
        {
            var connection = CreateConnection();

            var channel = connection.CreateModel();
            channel.ExchangeDeclare(
                exchange: EXCHANGE_NAME,
                type: ExchangeType.Topic,
                durable: true);

            return channel;
        }

        private void RabbitMqConsumer_Received(object sender, BasicDeliverEventArgs e)
        {
            object message = DeserializeMessage(e.Body, e.RoutingKey);

            OnMessagedReceived(new MessageReceivedEventArgs((Message)message));
        }
    }
}
