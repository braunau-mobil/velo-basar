using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    public enum ProductStatus
    {
        Available,
        Sold,
        Deleted,
        Stolen,
        PickedUp
    }

    public class Product : IValidatableObject
    {
        public int Id { get; set; }

        public string FrameNumber { get; set; }

        public string Color { get; set; }

        public string Brand { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string TireSize { get; set; }

        public decimal Price { get; set; }

        public ProductStatus Status { get; set; }

        public bool IsEmtpy()
        {
            return string.IsNullOrEmpty(Color)
                && string.IsNullOrEmpty(Brand)
                && string.IsNullOrEmpty(Description)
                && string.IsNullOrEmpty(Type)
                && string.IsNullOrEmpty(Type)
                && string.IsNullOrEmpty(TireSize);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (!IsEmtpy())
            {
                var requiredAttribute = new RequiredAttribute();

                result.AddIfNotNull(requiredAttribute.GetValidationResult(Brand, validationContext));
                result.AddIfNotNull(requiredAttribute.GetValidationResult(Type, validationContext));
                result.AddIfNotNull(requiredAttribute.GetValidationResult(Price, validationContext));
                result.AddIfNotNull(requiredAttribute.GetValidationResult(TireSize, validationContext));
                result.AddIfNotNull(requiredAttribute.GetValidationResult(Description, validationContext));
                result.AddIfNotNull(requiredAttribute.GetValidationResult(Color, validationContext));
            }

            return result;
        }
    }
}
