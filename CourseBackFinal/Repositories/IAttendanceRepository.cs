using CourseBackFinal.DTO;
using CourseBackFinal.Models;

namespace CourseBackFinal.Repositories
{
    public interface IAttendanceRepository
    {
        Task<ResponseObject> GetAbsencesForStudentForCourse(int courseId, string studentId);
        Task<ResponseObject> GetAbsencesCountForStudentForCourse(int courseId, string studentId);
        Task<ResponseObject> GetAbsencesCountForStudentForCourse(int courseId, string studentId, DateTime end, DateTime? start = null);
        Task<ResponseObject> EditAbsence(int absenceId, string studentId, EditAbsenceModel editAbsenceModel);

    }
}
