using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS_Project.Core.Models.Requests
{
    public class CityRequest
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PostalCode { get; set; }
        public List<Guid>? StreetIds { get; set; }
    }
}