using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BraunauMobil.VeloBasar.Models
{
    [Display(Name = "Artikel")]
    public class Product : IValidatableObject
    {
        public int Id { get; set; }

        [Display(Name = "Rahmennummer")]
        public string FrameNumber { get; set; }

        [Display(Name = "Farbe")]
        public string Color { get; set; }

        [Display(Name = "Marke")]
        public string Brand { get; set; }

        [Display(Name = "Beschreibung")]
        public string Description { get; set; }

        [Display(Name = "Typ")]
        public string Type { get; set; }

        [Display(Name = "Reifengröße")]
        public string TireSize { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Preis")]
        public decimal Price { get; set; }

        [Display(Name = "Status")]
        public ProductStatus Status { get; set; }

        [Display(Name = "Etikett")]
        public int? Label { get; set; }

        public bool Is(ProductStatus state)
        {
            return Status == state;
        }

        public bool IsNot(ProductStatus state)
        {
            return Status != state;
        }

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
