using Microsoft.AspNetCore.Mvc;
using Pricing.Domain;
using Pricing.Domain.Dtos;
using System.Threading.Tasks;

namespace Pricing.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TarrifController : ControllerBase
    {
        private readonly ITarrifService tarrifService;

        public TarrifController(ITarrifService tarrifService)
        {
            this.tarrifService = tarrifService;
        }

        // GET: api/Tarrif/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<TarrifDto> Get(int id)
        {
            return await this.tarrifService.GetTarrif(id);
        }

        // POST: api/Tarrif
        [HttpPost]
        public async Task Post(AddTarrifRequestDto request)
        {
            await tarrifService.AddTarrif(request);
        }

        // PUT: api/Tarrif
        [HttpPut]
        public async Task Put(UpdateTarrifRequestDto request)
        {
            await tarrifService.UpdateTarrif(request);
        }

        // DELETE: api/Tarrif/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await tarrifService.DeleteTarrif(id);
        }
    }
}
