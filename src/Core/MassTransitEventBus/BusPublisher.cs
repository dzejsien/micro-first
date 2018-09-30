using Core.Common.EventBus;
using MassTransit;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Core.MassTransitEventBus
{
    public class BusPublisher : IBusPublisher
    {
        private readonly IBusControl _busClient;

        public BusPublisher(IBusControl busClient)
        {
            _busClient = busClient;
        }

        public async Task PublishCommandAsync<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            //await _busClient.Publish(command);

            var sendToUri = new Uri($"rabbitmq://10.0.75.2:5672/{typeof(TCommand).FullName}");
            var endPoint = await _busClient.GetSendEndpoint(sendToUri);
            await endPoint.Send<TCommand>(command);
        }

        public async Task PublishEventAsync<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            await _busClient.Publish(@event);
        }
    }
}
