using LMS_Project.Common.Enums;
using System;

namespace LMS_Project.Core.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public TechnologiesEnum Technologies { get; set; }
        public LevelEnum Level { get; set; }
    }
}