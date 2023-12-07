using AutoMapper;
using LMS_Project.Core.Models;
using LMS_Project.Data.ModelDb;

namespace LMS_Project.Data.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<CityDbModel, City>().ReverseMap();
        }
    }
}