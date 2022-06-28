using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace CourseBackFinal.Models
{
    public class AppUser:IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Address { get; set; }

        public IList<CourseModel>? Courses { get; set; }
        public IList<ClassModel>? Classes { get; set; }
    }
}
