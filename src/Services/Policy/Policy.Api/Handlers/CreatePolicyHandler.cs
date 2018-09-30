using System.Threading.Tasks;
using Core.Common.EventBus;
using Core.EventBus;
using Core.Messages.Commands.Policies;
using Core.Messages.Events.Policies;
using MassTransit;
using Policy.DataAccess.SqlServer;
using Policy.Domain;

namespace Policy.Api.Handlers
{
    public class CreatePolicyHandler : ICommandHandler<CreatePolicyCommand>
    {
        private readonly IHandler _handler;
        private readonly IPolicyService _policyService;
        private readonly IBusPublisher _busPublisher;

        public CreatePolicyHandler(IHandler handler, IPolicyService policyService, IBusPublisher busPublisher)
        {
            _handler = handler;
            _policyService = policyService;
            _busPublisher = busPublisher;
        }

        public async Task HandleAsync(CreatePolicyCommand command)
        {
            await _handler.Handle(async () =>
            {
                var response = await _policyService.CreatePolicyAsync(command);
                await _busPublisher.PublishEventAsync(new PolicyCreatedEvent(response.PolicyId, command.ProductsCodes));
            })
            .OnError((ex) =>
            {
                throw ex;
            })
            .ExecuteAsync();
        }
    }

    public class CreatePolicyConsumer : IConsumer<CreatePolicyCommand>
    {
        private readonly ICommandHandler<CreatePolicyCommand> _handler;

        public CreatePolicyConsumer(ICommandHandler<CreatePolicyCommand> handler)
        {
            _handler = handler;
        }

        public async Task Consume(ConsumeContext<CreatePolicyCommand> context)
        {
            await _handler.HandleAsync(context.Message);
        }
    }
}
