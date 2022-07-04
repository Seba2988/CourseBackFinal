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
        public DbSet<AbsenceModel> Absences { get; set; }
        //public DbSet<CourseUser> CourseUsers { get; set; }

        /*protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            *//*builder.Entity<CourseModel>()
                .HasKey(c => c.Id);

            builder.Entity<CourseModel>().
                HasMany(c => c.Students)
                .WithMany(s => s.Courses)
                .UsingEntity<CourseUser>(j => j.HasKey(k => new { k.StudentId, k.CourseId }));*//*
            builder.Entity<CourseModel>()
                .HasMany(c => c.Students)
                .WithMany(s => s.Courses)
                .UsingEntity<CourseUser>(j => j.HasKey(k => new { k.CourseId, k.StudentId }))
                .HasKey(c => new { c.Id, c.ProfessorId });
            *//*builder.Entity<ClassModel>()
                .HasKey(c => new { c.Student, c.Course, c.Id });*//*
        }*/
    }
}
