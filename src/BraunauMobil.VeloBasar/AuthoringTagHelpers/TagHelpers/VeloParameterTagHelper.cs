using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace BraunauMobil.VeloBasar.AuthoringTagHelpers.TagHelpers
{
    [HtmlTargetElement(Attributes = "velo-parameter")]
    public class VeloParameterTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IHtmlGenerator _htmlGenerator;

        public VeloParameterTagHelper(IHtmlGenerator htmlGenerator, IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
            _htmlGenerator = htmlGenerator;
        }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }
        [HtmlAttributeName("velo-parameter")]
        public object Parameter { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            
            var tagHelper = CreateTagHelper(context.TagName);
            tagHelper.Process(context, output);
        }
        private TagHelper CreateTagHelper(string tagName)
        {
            if (tagName == "input" || tagName == "button")
            {
                return new FormActionTagHelper(_urlHelperFactory)
                {
                    RouteValues = RoutingHelper.ConvertToRoute(Parameter),
                    ViewContext = this.ViewContext
                };
            }

            return new AnchorTagHelper(_htmlGenerator)
            {
                RouteValues = RoutingHelper.ConvertToRoute(Parameter),
                ViewContext = this.ViewContext
            };
        }

    }
}
