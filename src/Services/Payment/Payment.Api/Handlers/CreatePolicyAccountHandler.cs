using System.Threading.Tasks;
using Core.Common.EventBus;
using Core.EventBus;
using Core.Messages.Commands.Payments;
using Core.Messages.Events.Payments;

namespace Payment.Api.Handlers
{
    public class CreatePolicyAccountHandler : ICommandHandler<CreatePolicyAccountCommand>
    {
        private readonly IHandler _handler;
        private readonly IBusPublisher _busPublisher;

        public CreatePolicyAccountHandler(IHandler handler, IBusPublisher busPublisher)
        {
            _handler = handler;
            _busPublisher = busPublisher;
        }

        public async Task HandleAsync(CreatePolicyAccountCommand command)
        {
            await _handler.Handle(async () =>
            {
                System.Console.WriteLine(command);

                await _busPublisher.PublishEventAsync(new PolicyAccountCreatedEvent(command.PolicyId, command.Number));
            })
            .ExecuteAsync();
        }
    }
}
