using LMS_Project.Data.Abstractions;
using LMS_Project.Data.ModelDb;

namespace LMS_Project.Data.Repositories
{
    public class FacultyBuildingRepository : GenericRepository<FacultyBuildingDbModel>, IFacultyBuildingRepository
    {
        public FacultyBuildingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}