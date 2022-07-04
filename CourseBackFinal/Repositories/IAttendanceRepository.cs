using CourseBackFinal.DTO;

namespace CourseBackFinal.Repositories
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<AbsenceDTO>> GetAbsencesForStudentForCourse(int courseId, string studentId);
    }
}
