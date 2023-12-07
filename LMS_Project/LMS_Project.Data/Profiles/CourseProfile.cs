using AutoMapper;
using LMS_Project.Core.Models;
using LMS_Project.Data.ModelDb;

namespace LMS_Project.Data.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CourseDbModel, Course>().ReverseMap();
        }
    }
}