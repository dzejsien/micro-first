using Core.Common.EventBus;
using System.Threading.Tasks;

namespace Core.Common.EventBus
{
    public interface IBusPublisher
    {
        Task PublishCommandAsync<TCommand>(TCommand command) where TCommand : class, ICommand;
        Task PublishEventAsync<TEvent>(TEvent @event) where TEvent : class, IEvent;
    }
}
