using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Payment.Domain;

namespace Payment.Api.Controllers
{
    [Route("v1/payments/{policyId}/accounts/")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        
        [HttpGet("{accountNumber}")]
        public async Task<IActionResult> Get(long policyId, string accountNumber)
        {
            // tips: if need more data from api, change to get whole account instead
            var balance = await _accountService.GetBalanceAsync(policyId, accountNumber);
            return Ok(balance);
        }
    }
}
