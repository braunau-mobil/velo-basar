using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Diagnostics.Contracts;

namespace BraunauMobil.VeloBasar.AuthoringTagHelpers.TagHelpers
{
    [HtmlTargetElement(Attributes = nameof(Condition))]
    public class ConditionTagHelper : TagHelper
    {
        public bool? Condition { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Contract.Requires(output != null);

            if (Condition == false || Condition == null)
            {
                output.SuppressOutput();
            }
        }
    }
}
