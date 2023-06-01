using System.ComponentModel.DataAnnotations;

namespace Codebridge_Test_REST_API.Validation
{
    public class StringToFloatAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is float)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid Data Type Error");
        }
    }
}
