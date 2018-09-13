using System.Threading.Tasks;

namespace Core.Common.EventBus
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}