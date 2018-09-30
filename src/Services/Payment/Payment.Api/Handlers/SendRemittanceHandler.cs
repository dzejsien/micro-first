using System.Threading.Tasks;
using Core.Common.EventBus;
using Core.Messages.Commands.Payments;
using MassTransit;
using Payment.Domain;

namespace Payment.Api.Handlers
{
    public class SendRemittanceHandler : ICommandHandler<SendRemittanceCommand>
    {
        private readonly IHandler _handler;
        private readonly IAccountService _accountService;

        public SendRemittanceHandler(IHandler handler, IAccountService accountService)
        {
            _handler = handler;
            _accountService = accountService;
        }

        public async Task HandleAsync(SendRemittanceCommand command)
        {
            await _handler.Handle(async () =>
            {
                await _accountService.SendRemittanceAsync(command);
            })
            // todo: error handling
            .ExecuteAsync();
        }
    }

    public class SendRemittanceCommandConsumer : IConsumer<SendRemittanceCommand>
    {
        private readonly ICommandHandler<SendRemittanceCommand> _handler;

        public SendRemittanceCommandConsumer(ICommandHandler<SendRemittanceCommand> handler)
        {
            _handler = handler;
        }

        public async Task Consume(ConsumeContext<SendRemittanceCommand> context)
        {
            await _handler.HandleAsync(context.Message);
        }
    }
}
