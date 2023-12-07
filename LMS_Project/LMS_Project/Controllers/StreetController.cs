using LMS_Project.Core.Models.Requests;
using LMS_Project.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace LMS_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreetController : Controller
    {
        private readonly IStreetService _streetService;

        public StreetController(IStreetService streetService)
        {
            _streetService = streetService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWithIncludesAsync()
        {
            var result = await _streetService.GetAllWithIncludesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStreetById(Guid id)
        {
            var result = await _streetService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddStreet([FromBody] StreetRequest request)
        {
            var result = await _streetService.AddAsync(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStreet([FromBody] StreetRequest request)
        {
            var result = await _streetService.UpdateAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStreet(Guid id)
        {
            await _streetService.DeleteAsync(id);
            return Ok();
        }
    }
}