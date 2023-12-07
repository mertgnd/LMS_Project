using LMS_Project.Data.ModelDb;
using Microsoft.EntityFrameworkCore;

namespace LMS_Project.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<StudentDbModel> Students { get; set; }
        public DbSet<ProfessorDbModel> Professors { get; set; }
        public DbSet<CourseDbModel> Courses { get; set; }
        public DbSet<StudentCoursesDbModel> StudentCourses { get; set; }
        public DbSet<CityDbModel> Cities { get; set; }
        public DbSet<StreetDbModel> Streets { get; set; }
    }
}