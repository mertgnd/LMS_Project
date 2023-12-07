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
        public ICollection<StudentCoursesDbModel> StudentCourses { get; set; }
    }
}