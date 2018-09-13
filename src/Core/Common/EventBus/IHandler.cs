using System;
using System.Threading.Tasks;

namespace Core.Common.EventBus
{
    public interface IHandler
    {
        IHandler Handle(Func<Task> handle);
        IHandler OnSuccess(Func<Task> onSuccess);
        IHandler OnError(Func<Exception, Task> onError);
        Task ExecuteAsync();
    }
}
