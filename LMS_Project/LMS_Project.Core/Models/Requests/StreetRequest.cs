using System;
using System.ComponentModel.DataAnnotations;

namespace LMS_Project.Core.Models.Requests
{
    public class StreetRequest
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Guid? CityId { get; set; }
    }
}