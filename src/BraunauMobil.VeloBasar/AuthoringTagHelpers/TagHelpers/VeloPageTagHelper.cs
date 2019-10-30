using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BraunauMobil.VeloBasar.AuthoringTagHelpers.TagHelpers
{
    [HtmlTargetElement(Attributes = "velo-page")]
    public class VeloPageTagHelper : TagHelper
    {
        private readonly IHtmlGenerator _htmlGenerator;

        public VeloPageTagHelper(IHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator;
        }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }
        [HtmlAttributeName("velo-page")]
        public VeloPage Page { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var anchorTagHelper = new AnchorTagHelper(_htmlGenerator)
            {
                Page = Page.Page,
                RouteValues = Utils.ConvertToRoute(Page.Parameter),
                ViewContext = ViewContext
            };
            anchorTagHelper.Process(context, output);
        }
    }
}
