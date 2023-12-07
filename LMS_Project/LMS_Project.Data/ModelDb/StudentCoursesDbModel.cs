using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMS_Project.Data.ModelDb
{
    public class StudentCoursesDbModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        [ForeignKey("StudentId")]
        public StudentDbModel Student { get; set; }
        public Guid CourseId { get; set; }
        [ForeignKey("CourseId")]
        public CourseDbModel Course { get; set; }
    }
}