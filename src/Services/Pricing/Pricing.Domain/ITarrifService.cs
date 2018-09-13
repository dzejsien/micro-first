using Pricing.Domain.Dtos;
using System.Threading.Tasks;

namespace Pricing.Domain
{
    public interface ITarrifService
    {
        Task AddTarrif(AddTarrifRequestDto request);
        Task UpdateTarrif(UpdateTarrifRequestDto request);
        Task DeleteTarrif(int id);
        Task<TarrifDto> GetTarrif(int id);
    }
}
