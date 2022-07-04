namespace CourseBackFinal.DTO
{
    public class CourseInStudentDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ProfessorId { get; set; }

        public DateTime StartingDate { get; set; }

        public DateTime EndingDate { get; set; }
    }
}
