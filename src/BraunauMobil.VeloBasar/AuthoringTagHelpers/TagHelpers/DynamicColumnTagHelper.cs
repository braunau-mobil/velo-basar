using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.AuthoringTagHelpers.TagHelpers
{
    [HtmlTargetElement(ParentTag = "dynamic-table")]
    public class DynamicColumnTagHelper : TagHelper
    {
        [HtmlAttributeName("property-name")]
        public string PropertyName { get; set; }
        [HtmlAttributeName("width")]
        public string Width { get; set; }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (context.Items[nameof(DynamicTableConfiguration)] is DynamicTableConfiguration configuration)
            {
                configuration.AddColumn(PropertyName, Width);
            }
            output.SuppressOutput();

            await Task.CompletedTask;
        }
    }
}
