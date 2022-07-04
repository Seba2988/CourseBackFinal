using CourseBackFinal.Helpers;
namespace CourseBackFinal.Models
{
    public class UpdateCourseModel
    {
        public string? Name { get; set; }
        public string? ProfessorId { get; set; }

//        [DateLessThan("EndingDate", ErrorMessage = "The starting date must be before the ending date")]
        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        /*public DayOfWeek? DayOfWeek { get; set; }
        public string? Hour { get; set; }*/
    }
}
