using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace BraunauMobil.VeloBasar.AuthoringTagHelpers.TagHelpers
{
    [HtmlTargetElement(Attributes = nameof(Condition))]
    public class ConditionTagHelper : TagHelper
    {
        public bool? Condition { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));

            if (Condition == false || Condition == null)
            {
                output.SuppressOutput();
            }
        }
    }
}
