using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BraunauMobil.VeloBasar.AuthoringTagHelpers.TagHelpers
{
    public class DynamicColumnConfiguration
    {
        public ModelMetadata Property { get; set; }
        public string Width { get; set; } = "auto";
    }
}
