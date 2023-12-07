using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS_Project.Core.Models.Requests
{
    public class StudentRequest
    {
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public List<Guid> CourseIds { get; set; }
    }
}