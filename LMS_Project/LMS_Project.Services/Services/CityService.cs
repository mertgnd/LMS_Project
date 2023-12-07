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

namespace LMS_Project.Services.Services
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IStreetRepository _streetRepository;
        private readonly IMapper _mapper;

        public CityService(ICityRepository cityRepository, IMapper mapper, IStreetRepository streetRepository)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
            _streetRepository = streetRepository;
        }

        public async Task<List<CityResponse>> GetAllWithIncludesAsync()
        {
            var result = new List<CityResponse>();

            var cityDbList = await _cityRepository.GetCityWithIncludesAsync();

            foreach (var cityDb in cityDbList)
            {
                var cityResponse = _mapper.Map<City>(cityDb);

                var response = new CityResponse() { City = cityResponse, Streets = null };

                if (cityDb.Streets.Count > 0)
                {
                    var streetList = new List<Street>();

                    foreach (var cityStreet in cityDb.Streets)
                    {
                        var street = _mapper.Map<Street>(cityStreet);

                        streetList.Add(street);
                    }

                    response.Streets = streetList;
                }

                result.Add(response);
            }

            return result;
        }

        public async Task<CityResponse> GetByIdAsync(Guid id)
        {
            var cityDb = await _cityRepository.GetByIdWithIncludesAsync(id);

            if (cityDb == null)
            {
                throw new Exception("City not found");
            }

            var cityResponse = _mapper.Map<City>(cityDb);

            var result = new CityResponse() { City = cityResponse, Streets = null };

            if (cityDb.Streets.Count > 0)
            {
                var streetList = new List<Street>();

                foreach (var cityStreet in cityDb.Streets)
                {
                    var street = _mapper.Map<Street>(cityStreet);

                    streetList.Add(street);
                }

                result.Streets = streetList;
            }

            return result;
        }

        public async Task<CityResponse> AddAsync(CityRequest request)
        {
            var cityDb = new CityDbModel
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                PostalCode = request.PostalCode,
                Streets = new List<StreetDbModel>()
            };

            var cityResponse = _mapper.Map<City>(cityDb);

            var result = new CityResponse
            {
                City = cityResponse,
                Streets = new List<Street>()
            };

            if (request.StreetIds != null && request.StreetIds.All(id => id != Guid.Empty))
            {
                var streetDbList = await _streetRepository.GetStreetsByIdsAsync(request.StreetIds);

                if (streetDbList.Count() != request.StreetIds.Count())
                {
                    throw new Exception("Not all received Street ID-s exist in the database!");
                }

                foreach (var streetDb in streetDbList)
                {
                    cityDb.Streets.Add(streetDb);

                    result.Streets.Add(_mapper.Map<Street>(streetDb));
                }
            }

            await _cityRepository.AddAsync(cityDb);

            return result;
        }

        public async Task<CityResponse> UpdateAsync(CityRequest request)
        {
            var existingCityDb = await _cityRepository.GetByIdWithIncludesAsync(request.Id);

            if (existingCityDb == null)
            {
                throw new Exception("City couldnt found!");
            }

            existingCityDb.Name = request.Name;
            existingCityDb.PostalCode = request.PostalCode;

            if (request.StreetIds != null && request.StreetIds.Any(id => id != Guid.Empty))
            {
                var cityDbList = await _streetRepository.GetStreetsByIdsAsync(request.StreetIds);

                if (cityDbList == null || cityDbList.Count() != request.StreetIds.Count())
                {
                    throw new Exception("Not all receive course Id-s exist in the database!");
                }

                existingCityDb.Streets = existingCityDb.Streets
                    .Where(s => request.StreetIds.Contains(s.Id))
                    .Union(cityDbList)
                    .ToList();
            }
            else
            {
                existingCityDb.Streets = new List<StreetDbModel>();
            }

            await _cityRepository.UpdateAsync(existingCityDb);

            var updatedCityResponse = _mapper.Map<CityDbModel, City>(existingCityDb);

            var associatedStreets = existingCityDb.Streets?
                .Select(streetDb => _mapper.Map<StreetDbModel, Street>(streetDb))
                .ToList() ?? new List<Street>();

            var result = new CityResponse
            {
                City = updatedCityResponse,
                Streets = associatedStreets
            };

            return result;
        }

        public async Task DeleteAsync(Guid id)
        {
            var cityDb = await _cityRepository.GetByIdAsync(id);

            if (cityDb == null)
            {
                throw new Exception("City not found");
            }

            var streets = await _streetRepository.GetStreetsByCityIdAsync(cityDb.Id);

            foreach (var street in streets)
            {
                await _streetRepository.DeleteAsync(street.Id);
            }

            await _cityRepository.DeleteAsync(id);
        }
    }
}