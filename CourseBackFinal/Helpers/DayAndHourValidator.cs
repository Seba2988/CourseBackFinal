using System.ComponentModel.DataAnnotations;
namespace CourseBackFinal.Helpers
{
    public class DayAndHourValidator : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = (DateTime)value;
            var currentDay = (int)currentValue.DayOfWeek;
            var currentHour = (int)currentValue.Hour;
            if (currentDay > 4 || currentHour < 8 || currentHour > 20)
                return new ValidationResult(ErrorMessage);
            return ValidationResult.Success;

        }
    }
}
