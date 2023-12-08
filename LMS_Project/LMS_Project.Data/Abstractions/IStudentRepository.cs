using LMS_Project.Core.Models.Response;
using LMS_Project.Data.ModelDb;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS_Project.Data.Abstractions
{
    public interface IStudentRepository : IGenericRepository<StudentDbModel>
    {
        Task<StudentDbModel> GetByIdWithIncludesAsync(Guid id);
        Task<List<StudentDbModel>> GetStudentsByCourseId(Guid courseId);
        Task<List<StudentDbModel>> GetStudentsWithIncludesAsync();
    }
}