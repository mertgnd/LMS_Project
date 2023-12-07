using LMS_Project.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace LMS_Project.Core.Models.Requests
{
    public class CourseRequest
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public TechnologiesEnum Technologies { get; set; }
        public LevelEnum Level { get; set; }
        public Guid? ProfessorId { get; set; }
    }
}