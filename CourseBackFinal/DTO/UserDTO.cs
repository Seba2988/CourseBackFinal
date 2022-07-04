using CourseBackFinal.Models;

namespace CourseBackFinal.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Address { get; set; }

        public IList<CourseInStudentDTO>? Courses { get; set; }
        public IList<AbsenceDTO>? Absences { get; set; }

    }
}
