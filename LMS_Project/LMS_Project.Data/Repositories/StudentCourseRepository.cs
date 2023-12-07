using LMS_Project.Data.Abstractions;
using LMS_Project.Data.ModelDb;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Project.Data.Repositories
{
    public class StudentCourseRepository : GenericRepository<StudentCoursesDbModel>, IStudentCourseRepository
    {
        public StudentCourseRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Task<IQueryable<StudentCoursesDbModel>> GetStudentCoursesByStudentId(Guid studentId)
        {
            var studentCourseDb = DbSet.Where(x => x.StudentId == studentId);
            return Task.FromResult(studentCourseDb);
        }

        public Task<IQueryable<StudentCoursesDbModel>> GetStudentCoursesByCourseId(Guid courseId)
        {
            var studentCourseDb = DbSet.Where(x => x.CourseId == courseId);
            return Task.FromResult(studentCourseDb);
        }

        public async Task<List<CourseDbModel>> GetCoursesByStudentId(Guid studentId)
        {
            var coursesDb = await DbContext.StudentCourses.Include(x => x.Course).Where(x => x.StudentId == studentId).Select(x => x.Course).ToListAsync();
            return coursesDb;
        }
    }
}