using LMS_Project.Core.Models.Requests;
using LMS_Project.Core.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS_Project.Services.Abstractions
{
    public interface IStudentService
    {
        Task<List<StudentResponse>> GetAllWithIncludesAsync();
        Task<StudentResponse> GetByIdAsync(Guid id);
        Task<List<StudentResponse>> GetStudentsByCourseIdAsync(Guid courseId);
        Task<StudentResponse> AddAsync(StudentRequest student);
        Task<StudentResponse> UpdateAsync(StudentRequest student);
        Task DeleteAsync(Guid id);
    }
}