using LMS_Project.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMS_Project.Data.ModelDb
{
    public class CourseDbModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public TechnologiesEnum Technologies { get; set; }
        [Required]
        public LevelEnum Level { get; set; }

        public Guid? ProfessorId { get; set; }
        [ForeignKey("ProfessorId")]
        public ProfessorDbModel Professor { get; set; }
        public ICollection<StudentCoursesDbModel> StudentCourses { get; set; }
    }
}