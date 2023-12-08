using System;

namespace LMS_Project.Core.Models
{
    public class FacultyBuilding
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StreetNumber { get; set; }
        public Guid? StreetId { get; set; }
    }
}