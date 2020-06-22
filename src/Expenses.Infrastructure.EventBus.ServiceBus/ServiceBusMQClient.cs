using Expenses.Domain.Events;
using Expenses.Infrastructure.EventBus.MessageQueue;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Infrastructure.EventBus.ServiceBus
{
    public class ServiceBusMQClient : IMQClient, IMQConsumer
    {
        private const string TopicName = "events";
        private const string EventName = "event-name";

        private readonly ILogger logger;
        private readonly ITopicClient topicClient;
        private readonly ISubscriptionClient subscriptionClient;

        public event EventHandler<MessageReceivedEventArgs> MessagedReceived;

        public ServiceBusMQClient(
            IConfiguration configuration,
            ILogger<ServiceBusMQClient> logger)
        {
            this.logger = logger;

            var connectionString = configuration.GetConnectionString("ServiceBus") ?? throw new ArgumentNullException("ServiceBus connection string missing!");

            topicClient = new TopicClient(connectionString, TopicName);
        }

        public async void Send<TEvent>(TEvent message) where TEvent : Event
        {
            var serviceBusMessage = new Microsoft.Azure.ServiceBus.Message(SerializeMessage(message));

            serviceBusMessage.UserProperties.Add(EventName, typeof(TEvent).AssemblyQualifiedName);

            // Send the message to the topic
            await topicClient.SendAsync(serviceBusMessage);
        }

        public void Start(string queueName)
        {
            // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
                AutoComplete = false
            };

            // Register the function that processes messages.
            subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);

            logger.LogInformation("Azure ServiceBus consumer initialized successfully.");
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        protected void OnMessagedReceived(MessageReceivedEventArgs e)
        {
            logger.LogDebug($"Azure ServiceBus message received");
            MessagedReceived?.Invoke(this, e);
        }
        private async Task ProcessMessagesAsync(Microsoft.Azure.ServiceBus.Message serviceBusMessage, CancellationToken token)
        {
            // Process the message.
            string message = DeserializeMessage(serviceBusMessage.Body);
            if (!serviceBusMessage.UserProperties.TryGetValue(EventName, out object eventName))
            {
                throw new ArgumentNullException(nameof(serviceBusMessage), "Missing event name from ServiceBus message");
            }
            
            OnMessagedReceived(new MessageReceivedEventArgs(message, eventName.ToString()));

            // Complete the message so that it is not received again.
            // This can be done only if the subscriptionClient is created in ReceiveMode.PeekLock mode (which is the default).
            await subscriptionClient.CompleteAsync(serviceBusMessage.SystemProperties.LockToken);

            // Note: Use the cancellationToken passed as necessary to determine if the subscriptionClient has already been closed.
            // If subscriptionClient has already been closed, you can choose to not call CompleteAsync() or AbandonAsync() etc.
            // to avoid unnecessary exceptions.
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        private byte[] SerializeMessage<T>(T message)
        {
            string serializedMessage = JsonSerializer.Serialize<T>(message);

            return Encoding.UTF8.GetBytes(serializedMessage);
        }

        private string DeserializeMessage(ReadOnlyMemory<byte> body)
        {
            var message = Encoding.UTF8.GetString(body.ToArray());

            return message;
        }
    }
}
