using CourseBackFinal.Models;
namespace CourseBackFinal.DTO
{
    public class AbsenceDTO
    {
        public int Id { get; set; }
        public string? StudentId { get; set; }
        public ClassDTO Class { get; set; }
        public bool IsPresent { get; set; }
        public string? ReasonOfAbsence { get; set; }
    }
}
