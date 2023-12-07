using LMS_Project.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS_Project.Data.ModelDb
{
    public class ProfessorDbModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public GenderEnum Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public ICollection<CourseDbModel> Courses { get; set; }
    }
}