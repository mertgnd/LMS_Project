using LMS_Project.Data.ModelDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Project.Data.Abstractions
{
    public interface IStudentCourseRepository : IGenericRepository<StudentCoursesDbModel>
    {
        Task<IQueryable<StudentCoursesDbModel>> GetStudentCoursesByStudentId(Guid studentId);
        Task<IQueryable<StudentCoursesDbModel>> GetStudentCoursesByCourseId(Guid courseId);
        Task<List<CourseDbModel>> GetCoursesByStudentId(Guid studentId);
    }
}