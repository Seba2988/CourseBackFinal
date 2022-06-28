using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CourseBackFinal.Models;
namespace CourseBackFinal.Data
{
    public class CourseManagementContext : IdentityDbContext<AppUser>
    {
        public CourseManagementContext(DbContextOptions<CourseManagementContext> options) : base(options)
        {
        }
        public DbSet<CourseModel> Courses { get; set; }
        public DbSet<ClassModel> Classes { get; set; }
    }
}
