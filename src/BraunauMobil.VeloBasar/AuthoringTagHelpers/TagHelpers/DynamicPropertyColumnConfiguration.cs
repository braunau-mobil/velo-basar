using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BraunauMobil.VeloBasar.AuthoringTagHelpers.TagHelpers
{
    public class DynamicPropertyColumnConfiguration : DynamicColumnConfiguration
    {
        public ModelMetadata Property { get; set; }
    }
}
