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
    public class ProfessorService : IProfessorService
    {
        private readonly IProfessorRepository _professorRepository;
        private readonly IMapper _mapper;
        private readonly ICourseRepository _courseRepository;

        public ProfessorService(IProfessorRepository professorRepository, IMapper mapper, ICourseRepository courseRepository)
        {
            _professorRepository = professorRepository;
            _mapper = mapper;
            _courseRepository = courseRepository;
        }

        public async Task<List<ProfessorResponse>> GetAllWithIncludesAsync()
        {
            var result = new List<ProfessorResponse>();

            var professorsDbList = await _professorRepository.GetProfessorWithIncludesAsync();

            foreach (var professorDb in professorsDbList)
            {
                var professorResponse = _mapper.Map<Professor>(professorDb);

                var response = new ProfessorResponse() { Professor = professorResponse, Courses = null };

                if (professorDb.Courses.Count > 0)
                {
                    var courseList = new List<Course>();

                    foreach (var professorCourse in professorDb.Courses)
                    {
                        var course = _mapper.Map<Course>(professorCourse);

                        courseList.Add(course);
                    }

                    response.Courses = courseList;
                }

                result.Add(response);
            }

            return result;
        }

        public async Task<ProfessorResponse> GetByIdAsync(Guid id)
        {
            var professorDb = await _professorRepository.GetByIdWithIncludesAsync(id);

            if (professorDb == null)
            {
                throw new NotFoundException("Professor not found");
            }

            var professorResponse = _mapper.Map<Professor>(professorDb);

            var response = new ProfessorResponse() { Professor = professorResponse, Courses = null };

            if (professorDb.Courses.Count > 0)
            {
                var courseList = new List<Course>();

                foreach (var professorCourse in professorDb.Courses)
                {
                    var course = _mapper.Map<Course>(professorCourse);

                    courseList.Add(course);
                }

                response.Courses = courseList;
            }

            return response;
        }

        public async Task<ProfessorResponse> AddAsync(Professor request)
        {
            var professorDb = new ProfessorDbModel
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                Gender = request.Gender,
                DateOfBirth = request.DateOfBirth,
            };

            var professorResponse = _mapper.Map<Professor>(professorDb);

            var result = new ProfessorResponse
            {
                Professor = professorResponse,
                Courses = new List<Course>()
            };

            await _professorRepository.AddAsync(professorDb);

            return result;
        }

        public async Task<ProfessorResponse> UpdateAsync(ProfessorRequest request)
        {
            var existingProfessorDb = await _professorRepository.GetByIdWithIncludesAsync(request.Id);

            if (existingProfessorDb == null)
            {
                throw new NotFoundException("Professor not found");
            }

            existingProfessorDb.FirstName = request.FirstName;
            existingProfessorDb.LastName = request.LastName;
            existingProfessorDb.Email = request.Email;
            existingProfessorDb.Gender = request.Gender;
            existingProfessorDb.DateOfBirth = request.DateOfBirth;
            existingProfessorDb.Phone = request.Phone;

            if (request.CourseIds != null && request.CourseIds.Any(id => id != Guid.Empty))
            {
                var coursesDbList = await _courseRepository.GetCoursesByIdsAsync(request.CourseIds);

                if (coursesDbList == null || coursesDbList.Count() != request.CourseIds.Count())
                {
                    throw new BadRequestException("Not all received Course ID-s exist in the database.");
                }

                existingProfessorDb.Courses = existingProfessorDb.Courses
                    .Where(c => request.CourseIds.Contains(c.Id))
                    .Union(coursesDbList)
                    .ToList();
            }
            else
            {
                existingProfessorDb.Courses = new List<CourseDbModel>();
            }

            await _professorRepository.UpdateAsync(existingProfessorDb);

            var updatedProfessorResponse = _mapper.Map<ProfessorDbModel, Professor>(existingProfessorDb);

            var associatedCourses = existingProfessorDb.Courses?
                .Select(courseDb => _mapper.Map<CourseDbModel, Course>(courseDb))
                .ToList() ?? new List<Course>();

            var result = new ProfessorResponse
            {
                Professor = updatedProfessorResponse,
                Courses = associatedCourses
            };

            return result;
        }

        public async Task DeleteAsync(Guid id)
        {
            var professorDb = _professorRepository.GetByIdAsync(id);

            if (professorDb == null)
            {
                throw new NotFoundException("Professor not found");
            }

            await _professorRepository.DeleteAsync(id);
        }
    }
}