using LMS_Project.Data.Abstractions;
using LMS_Project.Data.ModelDb;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Project.Data.Repositories
{
    public class StudentRepository : GenericRepository<StudentDbModel>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<StudentDbModel> GetByIdWithIncludesAsync(Guid id)
        {
            return await DbSet.Include(x => x.StudentCourses).ThenInclude(x => x.Course).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<StudentDbModel>> GetStudentsByCourseId(Guid courseId)
        {
            return await DbContext.StudentCourses.Include(x => x.Student).Where(x => x.CourseId == courseId).Select(x => x.Student).ToListAsync();
        }

        public async Task<List<StudentDbModel>> GetStudentsWithIncludesAsync()
        {
            return await DbSet.Include(x => x.StudentCourses).ThenInclude(x => x.Course).ToListAsync();
        }
    }
}