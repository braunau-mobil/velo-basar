using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.AuthoringTagHelpers.TagHelpers
{
    /// <summary>
    /// This class is mainly based on this https://github.com/MissaouiChedy/DynamicFormTagHelper
    /// </summary>
    public class DynamicFormTagHelper : TagHelper
    {
        private const string _inputClass = "col-10 form-control";

        [HtmlAttributeName("velo-model")]
        public ModelExpression Model { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlGenerator _htmlGenerator;
        private readonly HtmlEncoder _htmlEncoder;

        public DynamicFormTagHelper(IHtmlGenerator htmlGenerator, HtmlEncoder htmlEncoder)
        {
            _htmlGenerator = htmlGenerator;
            _htmlEncoder = htmlEncoder;
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (output == null) throw new ArgumentNullException(nameof(output));

            output.Content.AppendLine("====DynamicFormStart====");

            foreach (var property in Model.ModelExplorer.Properties)
            {
                await AppendPropertyGroupAsync(output.Content, property);
            }

            output.Content.AppendLine("====DynamicFormEnd====");
        }

        private async Task AppendPropertyGroupAsync(TagHelperContent content, ModelExplorer property)
        {
            if (property.Metadata.HasHiddenInputAttribute())
            {
                await AppendHiddenInputAsync(content, property);
            }
            else
            {
                content.AppendHtmlLine("<div class='form-group row'>");
                await AppendLabelAsync(content, property);
                if (property.Metadata.IsEnum)
                {
                    await AppendEnumSelectAsync(content, property);
                }
                else
                {
                    await AppendInputAsync(content, property);
                }
                content.AppendHtmlLine("</div>");
            }
        }

        private async Task AppendEnumSelectAsync(TagHelperContent content, ModelExplorer property)
        {
            var tagHelpers = new[]
            {
                new SelectTagHelper(_htmlGenerator)
                {
                    For = new ModelExpression(property.Metadata.PropertyName, property),
                    Items = property.Metadata.GetEnumSelectList(),
                    ViewContext = ViewContext
                }
            };

            var attributes = new TagHelperAttributeList
            {
                 new TagHelperAttribute("class", _inputClass)
            };

            var selectContent = await GetGeneratedContentFromTagHelpersAsync("select", TagMode.StartTagAndEndTag, tagHelpers, attributes);
            content.AppendHtmlLine(selectContent);
        }
        private async Task AppendHiddenInputAsync(TagHelperContent content, ModelExplorer property)
        {
            var attributes = new TagHelperAttributeList
            {
                new TagHelperAttribute("type", "hidden")
            };

            await AppendInputAsync(content, property, attributes);
        }
        private async Task AppendInputAsync(TagHelperContent content, ModelExplorer property)
        {
            var attributes = new TagHelperAttributeList
            {
                 new TagHelperAttribute("class", _inputClass)
            };
            await AppendInputAsync(content, property, attributes, true);
        }
        private async Task AppendInputAsync(TagHelperContent content, ModelExplorer property, TagHelperAttributeList attributes, bool addValidation = false)
        {
            var tagHelpers = new List<ITagHelper>
            {
                new InputTagHelper(_htmlGenerator)
                {
                    For = new ModelExpression(property.Metadata.PropertyName, property),
                    ViewContext = ViewContext
                },
            };

            if (addValidation)
            {
                tagHelpers.Add(new ValidationTagHelper()
                {
                    For = new ModelExpression(property.Metadata.PropertyName, property),
                    ViewContext = ViewContext
                });
            }

            var inputContent = await GetGeneratedContentFromTagHelpersAsync("input", TagMode.SelfClosing, tagHelpers, attributes);
            content.AppendHtmlLine(inputContent);
        }
        private async Task AppendLabelAsync(TagHelperContent content, ModelExplorer property)
        {
            if (property.Metadata.HasHiddenInputAttribute())
            {
                return;
            }

            var labelTagHelper = new LabelTagHelper(_htmlGenerator)
            {
                For = new ModelExpression(property.Metadata.PropertyName, property),
                ViewContext = ViewContext
            };

            var attributes = new TagHelperAttributeList
            {
                new TagHelperAttribute("class", "col-2 col-form-label")
            };            

            var labelContent = await GetGeneratedContentFromTagHelperAsync("label", TagMode.StartTagAndEndTag, labelTagHelper, attributes);
            content.AppendHtmlLine(labelContent);
        }

        private async Task<string> GetGeneratedContentFromTagHelpersAsync(string tagName, TagMode tagMode, IEnumerable<ITagHelper> tagHelpers, TagHelperAttributeList attributes)
        {
            var output = new TagHelperOutput(tagName, attributes, (useCached, encoder) => Task.Run<TagHelperContent>(() => new DefaultTagHelperContent()))
            {
                TagMode = tagMode
            };
            var context = new TagHelperContext(attributes, new Dictionary<object, object>(), Guid.NewGuid().ToString());

            foreach (var tagHelper in tagHelpers)
            {
                tagHelper.Init(context);
                await tagHelper.ProcessAsync(context, output);
            }

            return RenderTag(output);
        }
        private async Task<string> GetGeneratedContentFromTagHelperAsync(string tagName, TagMode tagMode, ITagHelper tagHelper, TagHelperAttributeList attributes) => await GetGeneratedContentFromTagHelpersAsync(tagName, tagMode, new[] { tagHelper }, attributes);

        private string RenderTag(IHtmlContent output)
        {
            using var writer = new StringWriter();
            output.WriteTo(writer, _htmlEncoder);
            return writer.ToString();
        }
    }
}
