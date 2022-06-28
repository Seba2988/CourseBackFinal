using System.ComponentModel.DataAnnotations;
namespace CourseBackFinal.Models
{
    public class CourseModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartingDate { get; set; }

        [Required]
        public DateTime EndingDate { get; set; }

        [Required]
        [Range(0, 4)]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        [RegularExpression("/ ^([8 - 9] | 0[8 - 9] | 1[0 - 9] | 2[0]):([0 - 5]?[0 - 9])$/")]
        public string Hour { get; set; }
        public IList<AppUser>? Students { get; set; }
        public IList<ClassModel>? Classes { get; set; }

    }
}
