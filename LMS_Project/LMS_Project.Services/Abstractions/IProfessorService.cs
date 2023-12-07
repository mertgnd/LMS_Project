using LMS_Project.Core.Models.Requests;
using LMS_Project.Core.Models.Response;
using LMS_Project.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS_Project.Services.Abstractions
{
    public interface IProfessorService
    {
        Task<List<ProfessorResponse>> GetAllWithIncludesAsync();
        Task<ProfessorResponse> GetByIdAsync(Guid id);
        Task<ProfessorResponse> AddAsync(Professor request);
        Task<ProfessorResponse> UpdateAsync(ProfessorRequest request);
        Task DeleteAsync(Guid id);
    }
}