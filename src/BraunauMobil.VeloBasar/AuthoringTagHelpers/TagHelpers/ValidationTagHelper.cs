using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text.Encodings.Web;

namespace BraunauMobil.VeloBasar.AuthoringTagHelpers.TagHelpers
{
    [HtmlTargetElement(Attributes = "velo-validation-for")]
    public class ValidationTagHelper : TagHelper
    {
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("velo-validation-for")]
        public ModelExpression For { get; set; }
        [HtmlAttributeName("velo-validation-condition")]
        public Func<bool> Condition { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Condition != null && !Condition())
            {
                return;
            }

            var state = ViewContext.ModelState[For.Name];
            if (state == null)
            {
                return;
            }
            else if (state.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
            {
                output.AddClass("is-valid", HtmlEncoder.Default);
            }
            else
            {
                output.AddClass("is-invalid", HtmlEncoder.Default);
            }
        }
    }
}
