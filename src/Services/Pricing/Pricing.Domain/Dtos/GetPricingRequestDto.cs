using System.Collections.Generic;

namespace Pricing.Domain.Dtos
{
    public class GetPricingRequestDto
    {
        public IList<string> ProductCodes { get; set; }
        public int InsuredAge { get; set; }
    }
}
