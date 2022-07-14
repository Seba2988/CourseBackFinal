using System.ComponentModel.DataAnnotations;
namespace CourseBackFinal.Models
{
    public class SigninModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
