using LMS_Project.Data.ModelDb;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS_Project.Data.Abstractions
{
    public interface ICityRepository : IGenericRepository<CityDbModel>
    {
        Task<List<CityDbModel>> GetCityWithIncludesAsync();
        Task<CityDbModel> GetByIdWithIncludesAsync(Guid id);
    }
}