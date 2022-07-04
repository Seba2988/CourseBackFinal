using System.ComponentModel.DataAnnotations;

namespace CourseBackFinal.Helpers
{
    public class DateLessThanAttribute:ValidationAttribute
    {
        private readonly string _comparisonProperty;
        public DateLessThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = (DateTime)value;
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
                throw new ArgumentException("Property with this name not found");
            var comprarisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);
            if (currentValue > comprarisonValue)
                return new ValidationResult(ErrorMessage);
            return ValidationResult.Success;
        }
    }
}
