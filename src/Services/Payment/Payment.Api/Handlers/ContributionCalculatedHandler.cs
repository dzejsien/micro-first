using System.Threading.Tasks;
using Core.Common.EventBus;
using Core.EventBus;
using Core.Messages.Events.Payments;
using Core.Messages.Events.Pricing;
using Payment.Domain;

namespace Payment.Api.Handlers
{
    public class ContributionCalculatedHandler : IEventHandler<ContributionCalculatedEvent>
    {
        private readonly IHandler _handler;
        private readonly IBusPublisher _busPublisher;
        private readonly IAccountService _accountService;

        public ContributionCalculatedHandler(IHandler handler, IBusPublisher busPublisher, IAccountService accountService)
        {
            _handler = handler;
            _busPublisher = busPublisher;
            _accountService = accountService;
        }

        public async Task HandleAsync(ContributionCalculatedEvent @event)
        {
            await _handler.Handle(async () =>
            {
                string number = await _accountService.CreatePolicyAccountAsync(@event);
                await _busPublisher.PublishEventAsync(new PolicyAccountCreatedEvent(@event.PolicyId, number));
            }).ExecuteAsync();
        }
    }
}
