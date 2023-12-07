using LMS_Project.Core.Models.Requests;
using LMS_Project.Core.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS_Project.Services.Abstractions
{
    public interface IStreetService
    {
        Task<List<StreetResponse>> GetAllWithIncludesAsync();
        Task<StreetResponse> GetByIdAsync(Guid id);
        Task<StreetResponse> AddAsync(StreetRequest request);
        Task<StreetResponse> UpdateAsync(StreetRequest request);
        Task DeleteAsync(Guid id);
    }
}