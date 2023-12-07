using LMS_Project.Data.Abstractions;
using LMS_Project.Data.ModelDb;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS_Project.Data.Repositories
{
    public class CityRepository : GenericRepository<CityDbModel>, ICityRepository
    {
        public CityRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<CityDbModel> GetByIdWithIncludesAsync(Guid id)
        {
            var result = await DbSet.Include(x => x.Streets).FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<List<CityDbModel>> GetCityWithIncludesAsync()
        {
            var result = await DbSet.Include(x => x.Streets).ToListAsync();
            return result;
        }
    }
}