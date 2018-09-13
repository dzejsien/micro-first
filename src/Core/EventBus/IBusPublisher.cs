using Core.Common.EventBus;
using System.Threading.Tasks;

namespace Core.EventBus
{
    public interface IBusPublisher
    {
        Task PublishCommandAsync<TCommand>(TCommand command) where TCommand : ICommand;
        Task PublishEventAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
