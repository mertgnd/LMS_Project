using LMS_Project.Core.Models.Requests;
using LMS_Project.Core.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS_Project.Services.Abstractions
{
    public interface ICityService
    {
        Task<List<CityResponse>> GetAllWithIncludesAsync();
        Task<CityResponse> GetByIdAsync(Guid id);
        Task<CityResponse> AddAsync(CityRequest request);
        Task<CityResponse> UpdateAsync(CityRequest request);
        Task DeleteAsync(Guid id);
    }
}