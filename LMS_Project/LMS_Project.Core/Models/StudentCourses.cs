using System;

namespace LMS_Project.Core.Models
{
    public class StudentCourses
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
    }
}