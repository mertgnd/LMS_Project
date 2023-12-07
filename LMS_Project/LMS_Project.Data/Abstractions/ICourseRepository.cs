using LMS_Project.Data.ModelDb;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS_Project.Data.Abstractions
{
    public interface ICourseRepository : IGenericRepository<CourseDbModel>
    {
        Task<CourseDbModel> GetByIdWithIncludesAsync(Guid id);
        Task<List<CourseDbModel>> GetCoursesByIdsAsync(List<Guid> courseIds);
        Task<List<CourseDbModel>> GetCourseWithIncludesAsync();
    }
}