using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MedicalSimulation.Core.Validation;

public class PhoneNumberValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return ValidationResult.Success;
        }

        var input = value.ToString()!;
        
        // Remove any non-digit characters for validation
        var digitsOnly = Regex.Replace(input, @"\D", "");
        
        // Must be exactly 10 digits
        if (digitsOnly.Length == 10 && Regex.IsMatch(digitsOnly, @"^\d{10}$"))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage ?? "Phone number must be exactly 10 digits.");
    }
}
