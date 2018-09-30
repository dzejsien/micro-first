using System.Threading.Tasks;
using Core.Common.EventBus;
using Core.EventBus;
using Core.Messages.Events.Policies;
using Core.Messages.Events.Pricing;
using MassTransit;

namespace Pricing.Api.Handlers
{
    public class PolicyCreatedHandler : IEventHandler<PolicyCreatedEvent>
    {
        private readonly IHandler _handler;
        private readonly IBusPublisher _busPublisher;

        public PolicyCreatedHandler(IHandler handler, IBusPublisher busPublisher)
        {
            _handler = handler;
            _busPublisher = busPublisher;
        }

        public async Task HandleAsync(PolicyCreatedEvent @event)
        {
            await _handler.Handle(async () =>
            {
                //todo: calcluate contribution
                decimal value = new System.Random().Next(100, 9999);
                
                await _busPublisher.PublishEventAsync(new ContributionCalculatedEvent(@event.PolicyId, value));
            }).ExecuteAsync();
        }
    }

    public class PolicyCreatedEventConsumer : IConsumer<PolicyCreatedEvent>
    {
        private readonly IEventHandler<PolicyCreatedEvent> _handler;

        public PolicyCreatedEventConsumer(IEventHandler<PolicyCreatedEvent> handler)
        {
            _handler = handler;
        }

        public async Task Consume(ConsumeContext<PolicyCreatedEvent> context)
        {
            await _handler.HandleAsync(context.Message);
        }
    }
}
