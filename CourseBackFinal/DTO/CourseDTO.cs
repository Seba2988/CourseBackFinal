using CourseBackFinal.Models;
namespace CourseBackFinal.DTO
{
    public class CourseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ProfessorId { get; set; }

        public DateTime StartingDate { get; set; }

        public DateTime EndingDate { get; set; }

        public IList<StudentInCourseDTO>? Students { get; set; }
        public IList<ClassDTO>? Classes { get; set; }
    }
}
