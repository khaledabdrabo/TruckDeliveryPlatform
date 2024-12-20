using System.ComponentModel.DataAnnotations;

namespace TruckDeliveryPlatform.Models.Validation
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime <= DateTime.Now)
                {
                    return new ValidationResult("Date must be in the future");
                }
            }
            return ValidationResult.Success;
        }
    }
} 