using Expenses.Domain.Events;

namespace Expenses.Infrastructure.EventBus.MessageQueue
{
    public interface IMQClient
    {
        void Send<TEvent>(TEvent message) where TEvent : Event;
    }
}
