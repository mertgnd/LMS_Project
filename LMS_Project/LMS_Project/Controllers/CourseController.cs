using LMS_Project.Core.Models.Requests;
using LMS_Project.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace LMS_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] CourseRequest request)
        {
            var result = await _courseService.AddAsync(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var result = await _courseService.GetAllWithIncludesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(Guid id)
        {
            var result = await _courseService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCourse([FromBody] CourseRequest request)
        {
            var result = await _courseService.UpdateAsync(request);
            return Ok(result);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            await _courseService.DeleteAsync(id);
            return Ok();
        }
    }
}