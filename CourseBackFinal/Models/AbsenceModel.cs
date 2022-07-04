using System.ComponentModel.DataAnnotations;
namespace CourseBackFinal.Models
{
    public class AbsenceModel
    {
        public int Id { get; set; }
        public AppUser Student { get; set; }
        public ClassModel Class { get; set; }

        [Required]
        public bool IsPresent { get; set; }
        public string? ReasonOfAbsence { get; set; }
    }
}
