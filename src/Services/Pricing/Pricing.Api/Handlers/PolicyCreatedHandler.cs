using System.Threading.Tasks;
using Core.Common.EventBus;
using Core.EventBus;
using Core.Messages.Events.Policies;
using Core.Messages.Events.Pricing;

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
}
