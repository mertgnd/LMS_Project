using LMS_Project.Common.Enums;
using System;

namespace LMS_Project.Core.Models
{
    public class Professor
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}