using LMS_Project.Data.Abstractions;
using LMS_Project.Data.ModelDb;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS_Project.Data.Repositories
{
    public class ProfessorRepository : GenericRepository<ProfessorDbModel>, IProfessorRepository
    {
        public ProfessorRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ProfessorDbModel> GetByIdWithIncludesAsync(Guid id)
        {
            var result = await DbSet.Include(x => x.Courses).FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<List<ProfessorDbModel>> GetProfessorWithIncludesAsync()
        {
            var result = await DbSet.Include(x => x.Courses).ToListAsync();
            return result;
        }
    }
}