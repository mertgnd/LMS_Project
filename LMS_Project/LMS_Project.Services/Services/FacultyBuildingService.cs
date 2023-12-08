using AutoMapper;
using LMS_Project.Common.Exceptions;
using LMS_Project.Core.Models;
using LMS_Project.Core.Models.Response;
using LMS_Project.Data.Abstractions;
using LMS_Project.Data.ModelDb;
using LMS_Project.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS_Project.Services.Services
{
    public class FacultyBuildingService : IFacultyBuildingService
    {
        private readonly IFacultyBuildingRepository _facultyBuildingRepository;
        private readonly IStreetRepository _streetRepository;
        private readonly IMapper _mapper;

        public FacultyBuildingService(IFacultyBuildingRepository facultyBuildingRepository, IMapper mapper, IStreetRepository streetRepository)
        {
            _facultyBuildingRepository = facultyBuildingRepository;
            _mapper = mapper;
            _streetRepository = streetRepository;
        }

        public Task<List<FacultyBuilding>> GetAllAsync()
        {
            var facultyBuildingDb = _facultyBuildingRepository.GetAll();

            var result = _mapper.Map<List<FacultyBuilding>>(facultyBuildingDb);

            return Task.FromResult(result);
        }

        public async Task<FacultyBuildingResponse> GetByIdAsync(Guid id)
        {
            var facultyBuildingDb = await _facultyBuildingRepository.GetByIdAsync(id) ?? 
                throw new NotFoundException($"ID: {id} Faculty Building is not exist!");

            return ResponseBuilder(facultyBuildingDb).Result;
        }

        public async Task<FacultyBuildingResponse> AddAsync(FacultyBuilding facultyBuilding)
        {
            var facultyBuildingDb = _mapper.Map<FacultyBuildingDbModel>(facultyBuilding);

            facultyBuildingDb.Id = Guid.NewGuid();

            if (facultyBuilding.StreetId != null)
            {
                facultyBuilding.StreetId = facultyBuildingDb.StreetId;
            }
            else
            {
                throw new BadRequestException("Street is required.");
            }

            var facultyBuildingDbResponse = await _facultyBuildingRepository.AddAsync(facultyBuildingDb);

            return ResponseBuilder(facultyBuildingDbResponse).Result;
        }

        public async Task<FacultyBuildingResponse> UpdateAsync(FacultyBuilding request)
        {
            var facultyBuildingDb = await _facultyBuildingRepository.GetByIdAsync(request.Id);

            if (facultyBuildingDb == null)
            {
                throw new NotFoundException($"No existing faculty building to update with ID: {facultyBuildingDb.Id}");
            }

            if (request.StreetId == null)
            {
                throw new BadRequestException("Street is required.");
            }

            _mapper.Map(request, facultyBuildingDb);

            var facultyBuildingDbResponse = await _facultyBuildingRepository.UpdateAsync(facultyBuildingDb);

            return ResponseBuilder(facultyBuildingDbResponse).Result;
        }

        public async Task DeleteAsync(Guid id)
        {
            var facultyBuildingDb = await _facultyBuildingRepository.GetByIdAsync(id);

            if (facultyBuildingDb == null)
            {
                throw new NotFoundException($"No existing faculty building to delete with ID: {id}");
            }

            await _facultyBuildingRepository.DeleteAsync(facultyBuildingDb);
        }

        private async Task<FacultyBuildingResponse> ResponseBuilder(FacultyBuildingDbModel facultyBuildingDb)
        {
            var facultyBuildingResponse = _mapper.Map<FacultyBuilding>(facultyBuildingDb);

            var streetDb = await _streetRepository.GetByIdWithIncludesAsync(facultyBuildingDb.StreetId);

            var streetResponse = _mapper.Map<Street>(streetDb);

            var cityResponse = _mapper.Map<City>(streetDb.City);

            var response = new FacultyBuildingResponse()
            {
                FacultyBuilding = facultyBuildingResponse,
                Street = streetResponse,
                City = cityResponse
            };

            return response;
        }
    }
}