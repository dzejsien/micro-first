using Core.EventBus;
using Core.Messages.Commands.Policies;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MicroDemo.Api.Controllers
{
    [ApiController]
    [Route("v1/polices")]
    public class PolicyController : BaseController
    {
        public PolicyController(IBusPublisher busPublisher) : base(busPublisher)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreatePolicyCommand command)
        {
            return await PublishAsync(command);
        }
    }
}
