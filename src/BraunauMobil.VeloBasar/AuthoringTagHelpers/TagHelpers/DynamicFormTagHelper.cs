using BraunauMobil.VeloBasar.Resources;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.AuthoringTagHelpers.TagHelpers
{
    /// <summary>
    /// This class is mainly based on this https://github.com/MissaouiChedy/DynamicFormTagHelper
    /// </summary>
    public class DynamicFormTagHelper : TagHelper
    {
        [HtmlAttributeName("velo-model")]
        public ModelExpression Model { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlLocalizer<SharedResource> _localizer;
        private readonly IHtmlGenerator _htmlGenerator;
        private readonly HtmlEncoder _htmlEncoder;

        public DynamicFormTagHelper(IHtmlGenerator htmlGenerator, HtmlEncoder htmlEncoder, IHtmlLocalizer<SharedResource> localizer)
        {
            _htmlGenerator = htmlGenerator;
            _htmlEncoder = htmlEncoder;
            _localizer = localizer;
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (output == null) throw new ArgumentNullException(nameof(output));

            var html = new Html(_localizer, _htmlGenerator, _htmlEncoder, ViewContext);

            output.Content.AppendLine("====DynamicFormStart====");

            foreach (var property in Model.ModelExplorer.Properties)
            {
                output.Content.AppendHtml(await html.FormRowAsync(property));
            }

            output.Content.AppendLine("====DynamicFormEnd====");
        }
    }
}
