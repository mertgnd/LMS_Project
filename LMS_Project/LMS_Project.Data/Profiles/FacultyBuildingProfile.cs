using AutoMapper;
using LMS_Project.Core.Models;
using LMS_Project.Data.ModelDb;

namespace LMS_Project.Data.Profiles
{
    public class FacultyBuildingProfile : Profile
    {
        public FacultyBuildingProfile()
        {
            CreateMap<FacultyBuildingDbModel, FacultyBuilding>().ReverseMap();
        }
    }
}