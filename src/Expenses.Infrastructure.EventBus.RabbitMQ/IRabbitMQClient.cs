using Expenses.Domain.Events;

namespace Expenses.Infrastructure.EventBus.RabbitMQ
{
    public interface IRabbitMQClient
    {
        void Send<TEvent>(TEvent message) where TEvent : Event;
    }
}
