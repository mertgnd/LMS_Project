using System.Collections.Generic;

namespace LMS_Project.Core.Models.Response
{
    public class StudentResponse
    {
        public Student Student { get; set; }
        public List<Course> Courses { get; set; }
    }
}