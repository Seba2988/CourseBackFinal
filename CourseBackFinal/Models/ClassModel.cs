using System.ComponentModel.DataAnnotations;
namespace CourseBackFinal.Models
{
    public class ClassModel
    {
        public int Id { get; set; }
        public CourseModel Course { get; set; }
        //public AppUser Student { get; set; }

        [Required]
        public DateTime Date { get; set; }
        public IList<AbsenceModel>? Absences { get; set; }

        /*[Required]
        public bool IsPresent { get; set; }
        public string? ReasonOfAbsence { get; set; }*/
    }
}
