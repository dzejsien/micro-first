using Core.Common.EventBus;
using Core.EventBus;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MicroDemo.Api.Controllers
{
    public abstract class BaseController : Controller
    {
        private static readonly string AcceptLanguageHeader = "accept-language";
        private static readonly string DefaultCulture = "en-us";

        private readonly IBusPublisher _busPublisher;

        public BaseController(IBusPublisher busPublisher)
        {
            _busPublisher = busPublisher;
        }

        protected string Culture
        {
            get
            {
                return Request.Headers.ContainsKey(AcceptLanguageHeader) ?
                               Request.Headers[AcceptLanguageHeader].First().ToLowerInvariant() :
                               DefaultCulture;
            }
        }

        protected async Task<IActionResult> PublishAsync<T>(T command, Guid? resourceId = null, string resource = "") where T : class, ICommand
        {
            // todo: publish context also
            var context = GetContext<T>(resourceId, resource);
            await _busPublisher.PublishCommandAsync(command);

            return Accepted(context);
        }

        protected CorrelationContext GetContext<T>(Guid? resourceId = null, string resource = "") where T : ICommand
        {
            if (!string.IsNullOrWhiteSpace(resource))
            {
                resource = $"{resource}/{resourceId}";
            }

            return CorrelationContext.Create<T>(Guid.NewGuid(), Guid.NewGuid(), resourceId ?? Guid.Empty, Request.Path.ToString(), Culture, resource);
        }
    }
}
