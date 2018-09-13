using Microsoft.AspNetCore.Mvc;
using Pricing.Domain;
using Pricing.Domain.Dtos;
using System.Threading.Tasks;

namespace Pricing.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PricingController : ControllerBase
    {
        private readonly IPricingService pricingService;

        public PricingController(IPricingService pricingService)
        {
            this.pricingService = pricingService;
        }

        // GET: api/Tarrif
        [HttpGet]
        public async Task<decimal> Get([FromQuery] GetPricingRequestDto request)
        {
            return await pricingService.GetPricing(request);
        }
    }
}
