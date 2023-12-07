using LMS_Project.Core.Models.Requests;
using LMS_Project.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace LMS_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCities()
        {
            var result = await _cityService.GetAllWithIncludesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCityById(Guid id)
        {
            var result = await _cityService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddCity([FromBody] CityRequest request)
        {
            var result = await _cityService.AddAsync(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCity([FromBody] CityRequest request)
        {
            var result = await _cityService.UpdateAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            await _cityService.DeleteAsync(id);
            return Ok();
        }
    }
}