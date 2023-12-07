using LMS_Project.Core.Models.Requests;
using LMS_Project.Core.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS_Project.Services.Abstractions
{
    public interface ICourseService
    {
        Task<List<CourseResponse>> GetAllWithIncludesAsync();
        Task<CourseResponse> GetByIdAsync(Guid id);
        Task<CourseResponse> AddAsync(CourseRequest course);
        Task<CourseResponse> UpdateAsync(CourseRequest course);
        Task DeleteAsync(Guid id);
    }
}