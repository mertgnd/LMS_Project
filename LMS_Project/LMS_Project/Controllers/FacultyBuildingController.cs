using LMS_Project.Core.Models.Requests;
using LMS_Project.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using LMS_Project.Core.Models;

namespace LMS_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultyBuildingController : ControllerBase
    {
        private readonly IFacultyBuildingService _facultyBuildingService;

        public FacultyBuildingController(IFacultyBuildingService facultyBuildingService)
        {
            _facultyBuildingService = facultyBuildingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFacultyBuildings()
        {
            var result = await _facultyBuildingService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFacultyBuildingById(Guid id)
        {
            var result = await _facultyBuildingService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddFacultyBuilding([FromBody] FacultyBuilding request)
        {
            var result = await _facultyBuildingService.AddAsync(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFacultyBuilding([FromBody] FacultyBuilding request)
        {
            var result = await _facultyBuildingService.UpdateAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacultyBuilding(Guid id)
        {
            await _facultyBuildingService.DeleteAsync(id);
            return Ok();
        }
    }
}
