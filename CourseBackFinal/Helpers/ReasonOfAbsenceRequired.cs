using System.ComponentModel.DataAnnotations;
namespace CourseBackFinal.Helpers
{
    public class ReasonOfAbsenceRequired:ValidationAttribute
    {
        private readonly string _comparisonProperty;
        public ReasonOfAbsenceRequired(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
                throw new ArgumentException("Property with this name not found");
            var isPresentValue = (bool)property.GetValue(validationContext.ObjectInstance);
            if (!isPresentValue && value == null)
                return new ValidationResult(ErrorMessage);
            return ValidationResult.Success;
        }
    }
}
