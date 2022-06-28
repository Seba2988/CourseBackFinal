namespace CourseBackFinal.Models
{
    public class UpdateUserModel
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Address { get; set; }
    }
}
