using Pricing.Domain.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pricing.Domain
{
    public class Price
    {
        [Key]
        public long PriceId { get; set; }

        [ForeignKey("TarrifId")]
        public Tarrif Tarrif{ get; set; }

        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
        public decimal Rate { get; set; }

        public static Price CreateFromDto(Tarrif tarrif, PriceDto request)
        {
            var newInstance = new Price();
            newInstance.AgeFrom = request.AgeFrom;
            newInstance.AgeTo = request.AgeTo;
            newInstance.Rate = request.Rate;
            newInstance.Tarrif = tarrif;
            return newInstance;
        }

        public PriceDto ToDto()
        {
            return new PriceDto
            {
                AgeFrom = AgeFrom,
                AgeTo = AgeTo,
                Rate = Rate
            };
        }
    }
}
