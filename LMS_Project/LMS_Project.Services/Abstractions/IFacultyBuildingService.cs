using LMS_Project.Core.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using LMS_Project.Core.Models;

namespace LMS_Project.Services.Abstractions
{
    public interface IFacultyBuildingService
    {
        Task<List<FacultyBuilding>> GetAllAsync();
        Task<FacultyBuildingResponse> GetByIdAsync(Guid id);
        Task<FacultyBuildingResponse> AddAsync(FacultyBuilding request);
        Task<FacultyBuildingResponse> UpdateAsync(FacultyBuilding request);
        Task DeleteAsync(Guid id);
    }
}