using System.Threading.Tasks;

namespace Core.Common.EventBus
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}