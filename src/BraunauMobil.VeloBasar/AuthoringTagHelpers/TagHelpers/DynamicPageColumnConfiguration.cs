using System;

namespace BraunauMobil.VeloBasar.AuthoringTagHelpers.TagHelpers
{
    public class DynamicPageColumnConfiguration : DynamicColumnConfiguration
    {
        public string PageText { get; set; }
        public Func<object, VeloPage> GetPage { get; set; }
    }
}
