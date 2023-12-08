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
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IStudentCourseRepository _studentCourseRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository studentRepository, IMapper mapper, IStudentCourseRepository studentCourseRepository, ICourseRepository courseRepository)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _studentCourseRepository = studentCourseRepository;
            _courseRepository = courseRepository;
        }

        public async Task<List<StudentResponse>> GetAllWithIncludesAsync()
        {
            var studentsDb = await _studentRepository.GetStudentsWithIncludesAsync();

            var studentsWithCourses = studentsDb.Select(studentDb =>
            {
                var student = _mapper.Map<StudentDbModel, Student>(studentDb);

                var courses = studentDb.StudentCourses.Select(sc => _mapper.Map<CourseDbModel, Course>(sc.Course)).ToList();

                var studentResponse = new StudentResponse
                {
                    Student = student,
                    Courses = courses
                };

                return studentResponse;
            }).ToList();

            return studentsWithCourses;
        }

        public async Task<List<StudentResponse>> GetStudentsByCourseIdAsync(Guid courseId)
        {
            var studentsDb = await _studentRepository.GetStudentsByCourseId(courseId);

            if (studentsDb == null || !studentsDb.Any())
            {
                return new List<StudentResponse>();
            }

            var studentsWithCourses = studentsDb
                .Where(studentDb => studentDb != null)
                .Select(studentDb =>
                {
                    var student = _mapper.Map<StudentDbModel, Student>(studentDb);

                    var courses = studentDb.StudentCourses
                        ?.Where(sc => sc.Course != null)
                        .Select(sc => _mapper.Map<CourseDbModel, Course>(sc.Course))
                        .ToList() ?? new List<Course>();

                    return new StudentResponse
                    {
                        Student = student,
                        Courses = courses
                    };
                })
                .ToList();

            return studentsWithCourses;
        }

        public async Task<StudentResponse> GetByIdAsync(Guid id)
        {
            var studentDb = await _studentRepository.GetByIdWithIncludesAsync(id);

            if (studentDb == null)
            {
                throw new NotFoundException($"{id} number student could not be found.");
            }

            var student = _mapper.Map<StudentDbModel, Student>(studentDb);

            var courses = studentDb.StudentCourses
                ?.Where(sc => sc.Course != null)
                .Select(sc => _mapper.Map<CourseDbModel, Course>(sc.Course))
                .ToList() ?? new List<Course>();

            var studentResponse = new StudentResponse
            {
                Student = student,
                Courses = courses
            };

            return studentResponse;
        }

        public async Task<StudentResponse> AddAsync(StudentRequest request)
        {
            var studentDb = new StudentDbModel
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                StudentCourses = new List<StudentCoursesDbModel>()
            };

            var studentResponse = _mapper.Map<Student>(studentDb);

            var result = new StudentResponse
            {
                Student = studentResponse,
                Courses = new List<Course>()
            };

            if (!(request.CourseIds == null || request.CourseIds.All(id => id == Guid.Empty)))
            {
                var coursesDbList = await _courseRepository.GetCoursesByIdsAsync(request.CourseIds);

                if (coursesDbList.Count() != request.CourseIds.Count())
                {
                    throw new BadRequestException("Not all received Course ID-s exist in the database.");
                }

                foreach (var course in coursesDbList)
                {
                    var studentCourse = new StudentCoursesDbModel
                    {
                        CourseId = course.Id,
                        StudentId = studentResponse.Id
                    };
                    studentDb.StudentCourses.Add(studentCourse);
                }
                result.Courses = coursesDbList.Select(courseDb => _mapper.Map<CourseDbModel, Course>(courseDb)).ToList();
            }

            await _studentRepository.AddAsync(studentDb);

            return result;
        }

        public async Task<StudentResponse> UpdateAsync(StudentRequest request)
        {
            var existingStudentDb = await _studentRepository.GetByIdWithIncludesAsync(request.Id);

            if (existingStudentDb == null)
            {
                throw new NotFoundException($"Student with id {request.Id} was not found");
            }

            existingStudentDb.FirstName = request.FirstName;
            existingStudentDb.LastName = request.LastName;

            if (existingStudentDb.StudentCourses != null && existingStudentDb.StudentCourses.Any())
            {
                foreach (var studentCourse in existingStudentDb.StudentCourses.ToList())
                {
                    await _studentCourseRepository.DeleteAsync(studentCourse.Id);
                }
                existingStudentDb.StudentCourses.Clear();
            }

            if (request.CourseIds != null && request.CourseIds.Any())
            {
                var studentCoursesDbModels = new List<StudentCoursesDbModel>();

                foreach (var courseId in request.CourseIds)
                {
                    var courseDbModel = await _courseRepository.GetByIdAsync(courseId);

                    if (courseDbModel != null)
                    {
                        var studentCourseDbModel = new StudentCoursesDbModel
                        {
                            Id = Guid.NewGuid(),
                            StudentId = existingStudentDb.Id,
                            CourseId = courseId,
                            Course = courseDbModel
                        };

                        studentCoursesDbModels.Add(studentCourseDbModel);
                        await _studentCourseRepository.AddAsync(studentCourseDbModel);
                    }
                }

                existingStudentDb.StudentCourses = studentCoursesDbModels;
            }

            var result = _mapper.Map<StudentDbModel, Student>(existingStudentDb);

            var studentResponse = new StudentResponse
            {
                Student = result,
                Courses = existingStudentDb.StudentCourses
                    .Select(sc => _mapper.Map<CourseDbModel, Course>(sc.Course))
                    .ToList(),
            };

            return studentResponse;
        }

        public async Task DeleteAsync(Guid id)
        {
            var studentDb = await _studentRepository.GetByIdAsync(id);

            if (studentDb == null)
            {
                throw new NotFoundException("Student not found");
            }

            var existingStudentCourses = await _studentCourseRepository.GetStudentCoursesByStudentId(studentDb.Id);
            foreach (var studentCourse in existingStudentCourses)
            {
                await _studentCourseRepository.DeleteAsync(studentCourse.Id);
            }

            await _studentRepository.DeleteAsync(id);
        }
    }
}