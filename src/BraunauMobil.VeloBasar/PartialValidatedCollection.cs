using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BraunauMobil.VeloBasar
{
    public class PartialValidatedCollection<T> : List<T>, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (typeof(T) == typeof(Product))
            {
                if (this.Cast<Product>().All(p => p.IsEmtpy()))
                {
                    result.Add(new ValidationResult("Es muss mindestens ein Produkt erfasst werden"));
                }
            }

            return result;
        }
    }
}
