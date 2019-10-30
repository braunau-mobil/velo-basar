using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BraunauMobil.VeloBasar.AuthoringTagHelpers.TagHelpers
{
    [HtmlTargetElement(Attributes = "velo-parameter")]
    public class VeloParameterTagHelper : TagHelper
    {
        private readonly IHtmlGenerator _htmlGenerator;

        public VeloParameterTagHelper(IHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator;
        }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }
        [HtmlAttributeName("velo-parameter")]
        public object Parameter { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var anchorTagHelper = new AnchorTagHelper(_htmlGenerator)
            {
                RouteValues = Utils.ConvertToRoute(Parameter),
                ViewContext = ViewContext
            };
            anchorTagHelper.Process(context, output);
        }
    }
}
