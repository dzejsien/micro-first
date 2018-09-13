using Pricing.Domain.Dtos;
using System.Threading.Tasks;

namespace Pricing.Domain
{
    public interface IPricingService
    {
        Task<decimal> GetPricing(GetPricingRequestDto request);
    }
}
