using LMS_Project.Core.Models;
using LMS_Project.Data.ModelDb;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS_Project.Data.Abstractions
{
    public interface IStreetRepository : IGenericRepository<StreetDbModel>
    {
        Task<List<StreetDbModel>> GetStreetsByIdsAsync(List<Guid> streetIds);
        Task<List<Street>> GetStreetsByCityIdAsync(Guid cityId);
        Task<List<StreetDbModel>> GetStreetWithIncludesAsync();
        Task<StreetDbModel> GetByIdWithIncludesAsync(Guid id);
    }
}