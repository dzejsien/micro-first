using System.Threading.Tasks;
using Core.Common.EventBus;
using Core.Messages.Commands.Payments;
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
}
