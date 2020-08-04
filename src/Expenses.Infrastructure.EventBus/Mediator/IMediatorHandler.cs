using Expenses.Domain.Commands;
using Expenses.Domain.Events;
using System.Threading.Tasks;

namespace Expenses.Infrastructure.EventBus.Mediator
{
    public interface IMediatorHandler
    {
        Task<bool> SendCommand<T>(T command) where T : Command;

        Task RaiseEvent<T>(T @event) where T : Event;
    }
}
