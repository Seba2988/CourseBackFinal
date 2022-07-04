using CourseBackFinal.Data;
using CourseBackFinal.DTO;
using CourseBackFinal.Models;
using CourseBackFinal.Helpers;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CourseBackFinal.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly CourseManagementContext _context;
        public AttendanceRepository(CourseManagementContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<AbsenceDTO>> GetAbsencesForStudentForCourse(int courseId, string studentId)
        {
            var absences = await ContextHelper.AbsencesGetter(courseId, studentId, _context).ToListAsync();
            return absences;
        }
    }
}
