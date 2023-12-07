using LMS_Project.Core.Models.Requests;
using LMS_Project.Core.Models;
using LMS_Project.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace LMS_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        private readonly IProfessorService _professorService;

        public ProfessorController(IProfessorService professorService)
        {
            _professorService = professorService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProfessor([FromBody] Professor professor)
        {
            var result = await _professorService.AddAsync(professor);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProfessors()
        {
            var result = await _professorService.GetAllWithIncludesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfessorById(Guid id)
        {
            var result = await _professorService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfessor([FromBody] ProfessorRequest request)
        {
            var result = await _professorService.UpdateAsync(request);
            return Ok(result);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfessor(Guid id)
        {
            await _professorService.DeleteAsync(id);
            return Ok();
        }
    }
}