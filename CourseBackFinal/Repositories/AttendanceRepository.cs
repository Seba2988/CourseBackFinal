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
        public async Task<ResponseObject> GetAbsencesForStudentForCourse(int courseId, string studentId)
        {
            var course = await ContextHelper.CourseGetter(courseId, _context);
            if (course == null) return new ResponseObject
            {
                Code = 400,
                Message = "No course found"
            };
            var student = course.Students.SingleOrDefault(s => s.Id == studentId);
            if (student == null) return new ResponseObject
            {
                Code = 400,
                Message = "The student has not been found in this course"
            };
            var absences = await ContextHelper.AbsencesGetter(courseId, studentId, _context)
                .OrderBy(a => a.Class.Date)
                .ToListAsync();
            if (absences.Count == 0) return new ResponseObject
            {
                Code = 201,
                Message = "No absences found"
            };
            return new ResponseObject
            {
                Result = absences
            };
        }
        public async Task<ResponseObject> GetAbsencesCountForStudentForCourse(int courseId, string studentId)
        {
            var result = await GetAbsencesForStudentForCourse(courseId, studentId);
            if (result.Message == null)
            {
                var absencesCount = ((List<AbsenceDTO>)result.Result)
                    .Where(a => a.IsPresent == false)
                    .Where(a => a.Class.Date < DateTime.Now)
                    .Count();
                var totalClasses = ((List<AbsenceDTO>)result.Result)
                    .Where(a => a.Class.Date < DateTime.Now)
                    .Count();
                return new ResponseObject
                {
                    Result = new ResponseObject
                    {
                        Code = 200,
                        Message = $"The student has been absent from {absencesCount} of {totalClasses} classes",
                        Result = ((List<AbsenceDTO>)result.Result)
                        .Where(a => a.Class.Date < DateTime.Now)
                    }
                };
            }
            return result;
        }
        public async Task<ResponseObject> GetAbsencesCountForStudentForCourse(
            int courseId,
            string studentId,
            DateTime? start = null,
            DateTime? end = null
            )
        {
            if (start != null && end < start) return new ResponseObject
            {
                Code = 400,
                Message = "The ending date cannot be before the starting date"
            };
            var result = await GetAbsencesForStudentForCourse(courseId, studentId);
            if (result.Message == null)
            {
                start = ((List<AbsenceDTO>)result.Result).FirstOrDefault().Class.Date > start || start == null
                    ? ((List<AbsenceDTO>)result.Result).FirstOrDefault().Class.Date
                    : start;
                end = ((List<AbsenceDTO>)result.Result).LastOrDefault().Class.Date < end || end == null
                    ? ((List<AbsenceDTO>)result.Result).LastOrDefault().Class.Date
                    : end;
                var absencesCount = ((List<AbsenceDTO>)result.Result)
                    .Where(a => a.IsPresent == false)
                    .Where(a => a.Class.Date >= start)
                    .Where(a => a.Class.Date <= end)
                    .Count();
                var totalClasses = ((List<AbsenceDTO>)result.Result)
                    .Where(a => a.Class.Date >= start)
                    .Where(a => a.Class.Date <= end)
                    .Count();
                return new ResponseObject
                {
                    Result = new ResponseObject
                    {
                        Code = 200,
                        Message = $"The student has been absent from {absencesCount} of {totalClasses} classes",
                        Result = ((List<AbsenceDTO>)result.Result)
                        .Where(a => a.Class.Date >= start)
                        .Where(a => a.Class.Date <= end)
                    }
                };
            }
            return result;
        }

        public async Task<ResponseObject> EditAbsence(int absenceId, string studentId, EditAbsenceModel editAbsenceModel)
        {
            var absence = await _context.Absences
                .Include(a => a.Class)
                .Include(a => a.Student)
                .Where(a => a.Student.Id == studentId)
                .SingleOrDefaultAsync(a => a.Id == absenceId);
            if (absence == null) return new ResponseObject
            {
                Code = 400,
                Message = "No absence found"
            };
            if (absence.Class.Date > DateTime.Now) return new ResponseObject
            {
                Code = 400,
                Message = "Can't edit the absence for a future class"
            };
            absence.IsPresent = editAbsenceModel.IsPresent;
            absence.ReasonOfAbsence = editAbsenceModel.ReasonOfAbsence;
            var result = await _context.SaveChangesAsync();
            if (result != 0)
            {
                var absenceToReturn = new AbsenceDTO
                {
                    Id = absence.Id,
                    StudentId = absence.Student.Id,
                    ReasonOfAbsence = absence.ReasonOfAbsence,
                    Class = new ClassDTO
                    {
                        Id = absence.Class.Id,
                        Date = absence.Class.Date
                    },
                    IsPresent = absence.IsPresent
                };
                return new ResponseObject
                {
                    Code = 200,
                    Result = absenceToReturn
                };
            }
            return new ResponseObject
            {
                Code = 400,
                Message = "No absence has been changed"
            };
        }


    }
}
