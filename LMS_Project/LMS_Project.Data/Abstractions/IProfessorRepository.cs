using LMS_Project.Data.ModelDb;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS_Project.Data.Abstractions
{
    public interface IProfessorRepository : IGenericRepository<ProfessorDbModel>
    {
        Task<ProfessorDbModel> GetByIdWithIncludesAsync(Guid id);
        Task<List<ProfessorDbModel>> GetProfessorWithIncludesAsync();
    }
}