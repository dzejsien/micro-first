using System.Threading.Tasks;
using Core.Common.EventBus;
using Core.Messages.Commands.Payments;
using MicroDemo.Api.Api;
using Microsoft.AspNetCore.Mvc;

namespace MicroDemo.Api.Controllers
{
    [ApiController]
    [Route("v1/payments")]
    public class PaymentController : BaseController
    {
        private readonly IPaymentApi _paymentApi;

        public PaymentController(IBusPublisher busPublisher, IPaymentApi paymentApi) : base(busPublisher)
        {
            _paymentApi = paymentApi;
        }

        [HttpGet("{policyId}/accounts/{accountNumber}")]
        public async Task<IActionResult> Get(long policyId, string accountNumber)
        {
            var result = await _paymentApi.GetAsync(policyId, accountNumber);
            return Ok(result);
        }

        [HttpPost("{policyId}/accounts/{accountNumber}")]
        public async Task<IActionResult> Post([FromBody]decimal value, [FromRoute] long policyId, [FromRoute] string accountNumber)
        {
            return await PublishAsync(new SendRemittanceCommand(value, policyId, accountNumber));
        }
    }
}
