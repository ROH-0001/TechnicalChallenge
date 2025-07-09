using System.ComponentModel.DataAnnotations;

namespace UserManagement.Shared.ValidationAttributes;
public class DateOfBirthAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
     
        if (value is DateTime dateOfBirth)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;

            //Base case if birthday hasnt happend this year
            if (dateOfBirth.Date > today.AddYears(-age))
                age--;

            if (dateOfBirth > today)
                return new ValidationResult("Date Of Birth cannot be in the future");
            if (age < 13)
                return new ValidationResult("Age must be at least 13 y/o");
            if (age > 120)
                return new ValidationResult("Age cannot exceed 120 y/o");
        }

        return ValidationResult.Success;
    }
}
