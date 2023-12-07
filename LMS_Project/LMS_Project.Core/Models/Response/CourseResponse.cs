using System.Collections.Generic;

namespace LMS_Project.Core.Models.Response
{
    public class CourseResponse
    {
        public Course Course { get; set; }
        public Professor Professor { get; set; }
        public List<Student> Students { get; set; }
    }
}