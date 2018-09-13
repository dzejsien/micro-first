using System;
using System.Threading.Tasks;

namespace Core.Common.EventBus
{
    public class Handler : IHandler
    {
        private Func<Task> _handle;
        protected Func<Task> _onSuccess;
        protected Func<Exception, Task> _onError;

        public IHandler Handle(Func<Task> handle)
        {
            _handle = handle;
            return this;
        }

        public IHandler OnSuccess(Func<Task> onSuccess)
        {
            _onSuccess = onSuccess;
            return this;
        }

        public IHandler OnError(Func<Exception, Task> onError)
        {
            _onError = onError;
            return this;
        }

        public virtual async Task ExecuteAsync()
        {
            bool isFailure = false;

            try
            {
                await _handle();
            }
            catch (Exception ex)
            {
                isFailure = true;
               
                if (_onError != null)
                {
                    await _onError.Invoke(ex);
                }
            }
            finally
            {
                if (!isFailure)
                {
                    if (_onSuccess != null)
                    {
                        await _onSuccess.Invoke();
                    }
                }
            }
        }
    }
}
