using System.Collections.Generic;

namespace LMS_Project.Core.Models.Response
{
    public class ProfessorResponse
    {
        public Professor Professor { get; set; }
        public List<Course> Courses { get; set; }
    }
}