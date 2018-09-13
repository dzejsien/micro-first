using Microsoft.EntityFrameworkCore;
using Pricing.Domain;
using Pricing.Domain.Dtos;
using System.Linq;
using System.Threading.Tasks;

namespace Pricing.DataAccess.PostgreSql
{
    public class PricingService : IPricingService
    {
        private readonly DomainModelPostgreSqlContext context;

        public PricingService(DomainModelPostgreSqlContext contex)
        {
            this.context = contex;
        }

        public async Task<decimal> GetPricing(GetPricingRequestDto request)
        {
            return await context.Prices
                .Where(p =>
                    request.ProductCodes.Any(x => x == p.Tarrif.ProductCode) &&
                    p.AgeFrom <= request.InsuredAge && p.AgeTo >= request.InsuredAge)
                .SumAsync(x => x.Rate);
        }
    }
}
