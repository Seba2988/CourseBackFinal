using System.ComponentModel.DataAnnotations;
using CourseBackFinal.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CourseBackFinal.Models
{
    [Index(nameof(Name), nameof(ProfessorId), IsUnique = true)]
    public class CourseModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ProfessorId { get; set; }

        [Required]
        [DateLessThan("EndingDate", ErrorMessage = "The starting date must be before the ending date")]
        [DayAndHourValidator(ErrorMessage = "The course cannot be in the night or the weekend")]
        public DateTime StartingDate { get; set; }

        [Required]
        public DateTime EndingDate { get; set; }

        /*[Required]
        [Range(0, 4)]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        [RegularExpression(@"^([8-9]|0[8-9]|1[0-9]|2[0]):([0-5]?[0-9])$")]
        public string Hour { get; set; }*/
        public IList<AppUser>? Students { get; set; }
        public IList<ClassModel>? Classes { get; set; }

    }
}
