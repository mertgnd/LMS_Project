using AutoMapper;
using LMS_Project.Core.Models.Requests;
using LMS_Project.Core.Models.Response;
using LMS_Project.Core.Models;
using LMS_Project.Data.Abstractions;
using LMS_Project.Data.ModelDb;
using LMS_Project.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using LMS_Project.Common.Exceptions;

namespace LMS_Project.Services.Services
{
    public class StreetService : IStreetService
    {
        private readonly IStreetRepository _streetRepository;
        private readonly IMapper _mapper;

        public StreetService(IStreetRepository streetRepository, IMapper mapper)
        {
            _streetRepository = streetRepository;
            _mapper = mapper;
        }

        public async Task<List<StreetResponse>> GetAllWithIncludesAsync()
        {
            var streetDb = await _streetRepository.GetStreetWithIncludesAsync();

            var streetsWithIncludes = streetDb.Select(streetDb =>
            {
                var street = _mapper.Map<StreetDbModel, Street>(streetDb);
                var city = _mapper.Map<CityDbModel, City>(streetDb.City);

                var streetResponse = new StreetResponse
                {
                    Street = street,
                    City = city,
                };

                return streetResponse;
            }).ToList();

            return streetsWithIncludes;
        }

        public async Task<StreetResponse> GetByIdAsync(Guid id)
        {
            var streetDb = await _streetRepository.GetByIdWithIncludesAsync(id);

            if (streetDb == null)
            {
                throw new NotFoundException("Street not found");
            }

            var street = _mapper.Map<StreetDbModel, Street>(streetDb);
            var city = _mapper.Map<CityDbModel, City>(streetDb.City);

            var streetResponse = new StreetResponse
            {
                Street = street,
                City = city,
            };

            return streetResponse;
        }

        public async Task<StreetResponse> AddAsync(StreetRequest request)
        {
            var streetDb = new StreetDbModel
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                CityId = request.CityId,
            };

            var addedStreetDb = await _streetRepository.AddAsync(streetDb);

            var addedStreet = await _streetRepository.GetByIdWithIncludesAsync(addedStreetDb.Id);

            var mappedStreet = _mapper.Map<StreetDbModel, Street>(addedStreet);

            City city;

            if (addedStreet?.City != null)
            {
                city = _mapper.Map<CityDbModel, City>(addedStreet.City);
            }
            else
            {
                city = null;
            }

            var streetResponse = new StreetResponse
            {
                Street = mappedStreet,
                City = city
            };

            return streetResponse;
        }

        public async Task<StreetResponse> UpdateAsync(StreetRequest request)
        {
            var existingStreetDb = await _streetRepository.GetByIdWithIncludesAsync(request.Id);

            if (existingStreetDb == null)
            {
                throw new NotFoundException("Street not found");
            }

            existingStreetDb.Name = request.Name;
            existingStreetDb.CityId = request.CityId;

            await _streetRepository.UpdateAsync(existingStreetDb);

            var updatedStreet = await _streetRepository.GetByIdWithIncludesAsync(request.Id);

            var mappedStreet = _mapper.Map<StreetDbModel, Street>(updatedStreet);

            City city;

            if (updatedStreet?.City != null)
            {
                city = _mapper.Map<CityDbModel, City>(updatedStreet.City);
            }
            else
            {
                city = null;
            }

            var streetResponse = new StreetResponse
            {
                Street = mappedStreet,
                City = city
            };

            return streetResponse;
        }

        public async Task DeleteAsync(Guid id)
        {
            var streetDb = await _streetRepository.GetByIdAsync(id);

            if (streetDb == null)
            {
                throw new NotFoundException("Professor not found");
            }

            await _streetRepository.DeleteAsync(id);
        }
    }
}