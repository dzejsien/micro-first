using Microsoft.EntityFrameworkCore;
using Pricing.Domain;
using Pricing.Domain.Dtos;
using System;
using System.Threading.Tasks;

namespace Pricing.DataAccess.PostgreSql
{
    public class TarrifService : ITarrifService
    {
        private readonly DomainModelPostgreSqlContext context;

        public TarrifService(DomainModelPostgreSqlContext contex)
        {
            this.context = contex;
        }

        public async Task AddTarrif(AddTarrifRequestDto request)
        {
            var exists = await context.Tarrifs.Include(t => t.Prices).AnyAsync(t => t.ProductCode == request.ProductCode);

            if (exists)
            {
                throw new ArgumentException($"Tarrif with Product Code: {request.ProductCode} already exists");
            }

            context.Tarrifs.Add(Tarrif.CreateFromDto(request));
            await context.SaveChangesAsync();
        }

        public async Task DeleteTarrif(int id)
        {
            var entity = await context.Tarrifs.Include(t => t.Prices).FirstOrDefaultAsync(t => t.TarrifId == id);

            if (entity == null)
            {
                throw new ArgumentException($"Tarrif with id: {id} not found");
            }

            context.Prices.RemoveRange(entity.Prices);
            context.Tarrifs.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<TarrifDto> GetTarrif(int id)
        {
            var entity = await context.Tarrifs.Include(s => s.Prices).FirstOrDefaultAsync(t => t.TarrifId == id);

            if (entity == null)
            {
                throw new ArgumentException($"Tarrif with id: {id} not found");
            }

            return entity.ToDto();
        }

        public async Task UpdateTarrif(UpdateTarrifRequestDto request)
        {
            var entity = await context.Tarrifs.Include(s => s.Prices).FirstOrDefaultAsync(t => t.ProductCode == request.ProductCode);

            if (entity == null)
            {
                throw new ArgumentException($"Tarrif with ProductCode: {request.ProductCode} not found");
            }

            context.Prices.RemoveRange(entity.Prices);
            entity.Prices.Clear();

            foreach (var price in request.Prices)
            {
                entity.AddPrice(Price.CreateFromDto(entity, price));
            }

            context.Tarrifs.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
