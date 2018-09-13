using Core.Common.EventBus;
using RawRabbit;
using System.Threading.Tasks;

namespace Core.EventBus
{
    public class BusPublisher : IBusPublisher
    {
        private readonly IBusClient<CorrelationContext> _busClient;

        public BusPublisher(IBusClient<CorrelationContext> busClient)
        {
            _busClient = busClient;
        }

        public async Task PublishCommandAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            await _busClient.PublishAsync(command);
        }

        public async Task PublishEventAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            await _busClient.PublishAsync(@event);
        }
    }
}
