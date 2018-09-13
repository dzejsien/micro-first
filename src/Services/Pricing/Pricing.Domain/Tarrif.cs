using Pricing.Domain.Dtos;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Pricing.Domain
{
    public class Tarrif
    {
        [Key]
        public long TarrifId { get; set; }
        public string ProductCode { get; set; }
        public IList<Price> Prices { get; set; }

        public Tarrif()
        {
            Prices = new List<Price>();
        }

        public static Tarrif CreateFromDto(AddTarrifRequestDto request)
        {
            var newInstance = new Tarrif();
            newInstance.ProductCode = request.ProductCode;

            foreach (var price in request.Prices)
            {
                newInstance.AddPrice(Price.CreateFromDto(newInstance, price));
            }

            return newInstance;
        }

        public TarrifDto ToDto()
        {
            return new TarrifDto
            {
                ProductCode = ProductCode,
                Prices = Prices.Select(x => x.ToDto()).ToList()
            };
        }

        public void AddPrice(Price newPrice)
        {
            Prices.Add(newPrice);
        }
    }
}
