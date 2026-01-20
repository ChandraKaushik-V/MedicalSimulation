using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MedicalSimulation.Core.Validation;

public class LettersOnlyAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return ValidationResult.Success;
        }

        var input = value.ToString()!;
        
        // Allow only letters and spaces
        if (Regex.IsMatch(input, @"^[a-zA-Z\s]+$"))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage ?? "Only letters and spaces are allowed.");
    }
}
