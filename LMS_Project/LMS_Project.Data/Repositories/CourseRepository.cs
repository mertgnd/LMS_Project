using LMS_Project.Data.Abstractions;
using LMS_Project.Data.ModelDb;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Project.Data.Repositories
{
    public class CourseRepository : GenericRepository<CourseDbModel>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<CourseDbModel>> GetCourseWithIncludesAsync()
        {
            var result = await DbSet.Include(c => c.Professor).Include(c => c.StudentCourses).ThenInclude(sc => sc.Student).ToListAsync();
            return result;
        }

        public async Task<CourseDbModel> GetByIdWithIncludesAsync(Guid id)
        {
            var result = await DbSet.Include(x => x.StudentCourses).ThenInclude(x => x.Student).Include(x => x.Professor).FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<List<CourseDbModel>> GetCoursesByIdsAsync(List<Guid> courseIds)
        {
            var result = await DbSet.Where(course => courseIds.Contains(course.Id)).ToListAsync();
            return result;
        }
    }
}