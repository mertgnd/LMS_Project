using LMS_Project.Core.Models.Requests;
using LMS_Project.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace LMS_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var result = await _studentService.GetAllWithIncludesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(Guid id)
        {
            var result = await _studentService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("studentByCourseId/{courseId}")]
        public async Task<IActionResult> GetStudentsByCourseId(Guid courseId)
        {
            var result = await _studentService.GetStudentsByCourseIdAsync(courseId);
            return Ok(result);
        }

        [HttpGet("/gender-statistics")]
        public async Task<IActionResult> GetStudentGenderStatistic()
        {
            var result = await _studentService.GetStudentGenderStatisticsAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] StudentRequest request)
        {
            var result = await _studentService.AddAsync(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudent([FromBody] StudentRequest request)
        {
            var result = await _studentService.UpdateAsync(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            await _studentService.DeleteAsync(id);
            return Ok();
        }
    }
}