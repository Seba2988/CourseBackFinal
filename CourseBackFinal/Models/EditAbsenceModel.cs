using System.ComponentModel.DataAnnotations;
using CourseBackFinal.Helpers;
namespace CourseBackFinal.Models
{
    public class EditAbsenceModel
    {
        [Required]
        public bool IsPresent { get; set; }

        [ReasonOfAbsenceRequired("IsPresent", ErrorMessage = "If the student is not present, must be a reason")]
        public string? ReasonOfAbsence { get; set; }
    }
}
