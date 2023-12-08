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
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly IStudentCourseRepository _studentCourseRepository;

        public CourseService(ICourseRepository courseRepository, IMapper mapper, IStudentCourseRepository studentCourseRepository)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
            _studentCourseRepository = studentCourseRepository;
        }

        public async Task<List<CourseResponse>> GetAllWithIncludesAsync()
        {
            var coursesDb = await _courseRepository.GetCourseWithIncludesAsync();

            var coursesWithIncludes = coursesDb.Select(courseDb =>
            {
                var course = _mapper.Map<CourseDbModel, Course>(courseDb);
                var professor = _mapper.Map<ProfessorDbModel, Professor>(courseDb.Professor);
                var students = courseDb.StudentCourses?.Select(sc => _mapper.Map<StudentDbModel, Student>(sc.Student)).ToList();

                var courseResponse = new CourseResponse
                {
                    Course = course,
                    Professor = professor,
                    Students = students
                };

                return courseResponse;
            }).ToList();

            return coursesWithIncludes;
        }

        public async Task<CourseResponse> GetByIdAsync(Guid id)
        {
            var courseDb = await _courseRepository.GetByIdWithIncludesAsync(id);

            if (courseDb == null)
            {
                throw new NotFoundException("Course not found");
            }

            var course = _mapper.Map<CourseDbModel, Course>(courseDb);
            var professor = _mapper.Map<ProfessorDbModel, Professor>(courseDb.Professor);
            var students = courseDb.StudentCourses?.Select(sc => _mapper.Map<StudentDbModel, Student>(sc.Student)).ToList();

            var courseResponse = new CourseResponse
            {
                Course = course,
                Professor = professor,
                Students = students
            };

            return courseResponse;
        }

        public async Task<CourseResponse> AddAsync(CourseRequest request)
        {
            var courseDb = new CourseDbModel
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Technologies = request.Technologies,
                Level = request.Level,
                ProfessorId = request.ProfessorId
            };

            var addedCourseDb = await _courseRepository.AddAsync(courseDb);

            var addedCourse = await _courseRepository.GetByIdWithIncludesAsync(addedCourseDb.Id);

            var mappedCourse = _mapper.Map<CourseDbModel, Course>(addedCourse);

            Professor professor;

            if (addedCourse?.Professor != null)
            {
                professor = _mapper.Map<ProfessorDbModel, Professor>(addedCourse.Professor);
            }
            else
            {
                professor = null;
            }

            var courseResponse = new CourseResponse
            {
                Course = mappedCourse,
                Professor = professor
            };

            return courseResponse;
        }

        public async Task<CourseResponse> UpdateAsync(CourseRequest request)
        {
            var existingCourseDb = await _courseRepository.GetByIdWithIncludesAsync(request.Id);

            if (existingCourseDb == null)
            {
                throw new NotFoundException("Course not found");
            }

            existingCourseDb.Name = request.Name;
            existingCourseDb.Description = request.Description;
            existingCourseDb.Price = request.Price;
            existingCourseDb.Technologies = request.Technologies;
            existingCourseDb.Level = request.Level;

            existingCourseDb.ProfessorId = request.ProfessorId;

            await _courseRepository.UpdateAsync(existingCourseDb);

            var updatedCourseWithIncludes = await _courseRepository.GetByIdWithIncludesAsync(request.Id);

            var mappedCourse = _mapper.Map<CourseDbModel, Course>(updatedCourseWithIncludes);

            Professor professor;

            if (updatedCourseWithIncludes?.Professor != null)
            {
                professor = _mapper.Map<ProfessorDbModel, Professor>(updatedCourseWithIncludes.Professor);
            }
            else
            {
                professor = null;
            }

            List<Student> students = new List<Student>();

            if (updatedCourseWithIncludes != null && updatedCourseWithIncludes.StudentCourses != null)
            {
                students = updatedCourseWithIncludes.StudentCourses
                    .Select(sc => _mapper.Map<StudentDbModel, Student>(sc.Student))
                    .ToList();
            }

            var courseResponse = new CourseResponse
            {
                Course = mappedCourse,
                Professor = professor,
                Students = students
            };

            return courseResponse;
        }

        public async Task DeleteAsync(Guid id)
        {
            var courseDb = await _courseRepository.GetByIdAsync(id);

            if (courseDb == null)
            {
                throw new NotFoundException("Course not found");
            }

            var existingStudentCourses = await _studentCourseRepository.GetStudentCoursesByCourseId(courseDb.Id);
            foreach (var studentCourse in existingStudentCourses)
            {
                await _studentCourseRepository.DeleteAsync(studentCourse);
            }

            await _courseRepository.DeleteAsync(id);
        }
    }
}