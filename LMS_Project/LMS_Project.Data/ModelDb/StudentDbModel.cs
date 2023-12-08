using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMS_Project.Data.ModelDb
{
    public class StudentDbModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [MaxLength(150)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(150)]
        [Required]
        public string LastName { get; set; }
        [MaxLength(150)]
        [Required]
        public string EMail { get; set; }
        [MaxLength(150)]
        [Required]
        public string Phone { get; set; }
        [MaxLength(150)]
        [Required]
        public string Gender { get; set; }
        [MaxLength(150)]
        [Required]
        public string DateOfBirth { get; set; }
        public ICollection<StudentCoursesDbModel> StudentCourses { get; set; }
    }
}