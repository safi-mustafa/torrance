using System;
using System.ComponentModel.DataAnnotations;

public class RequiredNotNullAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value != null)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult($"{validationContext.DisplayName} is required and cannot be null.");
    }
}
