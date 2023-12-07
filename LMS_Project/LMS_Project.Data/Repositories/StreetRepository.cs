using LMS_Project.Core.Models;
using LMS_Project.Data.Abstractions;
using LMS_Project.Data.ModelDb;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Project.Data.Repositories
{
    public class StreetRepository : GenericRepository<StreetDbModel>, IStreetRepository
    {
        public StreetRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<StreetDbModel>> GetStreetWithIncludesAsync()
        {
            var result = await DbSet.Include(c => c.City).ToListAsync();
            return result;
        }

        public async Task<StreetDbModel> GetByIdWithIncludesAsync(Guid id)
        {
            var result = await DbSet.Include(x => x.City).FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<List<StreetDbModel>> GetStreetsByIdsAsync(List<Guid> streetIds)
        {
            var result = await DbSet.Where(street => streetIds.Contains(street.Id)).ToListAsync();
            return result;
        }

        public async Task<List<Street>> GetStreetsByCityIdAsync(Guid cityId)
        {
            var streetDbModels = await DbContext.Streets
                .Where(s => s.CityId == cityId)
                .ToListAsync();

            var streets = streetDbModels.Select(streetDbModel => new Street
            {
                Id = streetDbModel.Id,
                Name = streetDbModel.Name,
            }).ToList();

            return streets;
        }
    }
}