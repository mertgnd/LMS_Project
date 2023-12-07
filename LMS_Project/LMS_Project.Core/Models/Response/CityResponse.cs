using System.Collections.Generic;

namespace LMS_Project.Core.Models.Response
{
    public class CityResponse
    {
        public City City { get; set; }
        public List<Street> Streets { get; set; }
    }
}