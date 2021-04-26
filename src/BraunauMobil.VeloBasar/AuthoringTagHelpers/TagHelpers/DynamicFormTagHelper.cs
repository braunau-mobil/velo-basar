using BraunauMobil.VeloBasar.Pages.Generic;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.AuthoringTagHelpers.TagHelpers
{
    /// <summary>
    /// This class is mainly based on this https://github.com/MissaouiChedy/DynamicFormTagHelper
    /// </summary>
    public class DynamicFormTagHelper : TagHelper
    {
        public ModelExpression PageModel { get; set; }
        public IHtmlContent TitleText { get; set; }

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
            var listPageModel = PageModel.Model as IEditPageModel;

            var buttonContainer = html.Div("d-flex justify-content-end");
            buttonContainer.InnerHtml.AppendHtml(await html.SaveButtonAsync(listPageModel.ListPage()));
            buttonContainer.InnerHtml.AppendHtml(await html.CancelButtonAsync(listPageModel.ListPageOrigin()));

            var cardHeader = html.CardHeader(html.CardHeaderTitle(TitleText), buttonContainer);
            var propertyRows = new List<IHtmlContent>();
            foreach (var property in GetItemModel().Properties)
            {
                if (property.IncludeInDynamicForm())
                {
                    propertyRows.Add(await html.FormRowAsync(property));
                }
            }
            var cardBody = html.EditCardBody(propertyRows.ToArray());
            var card = html.Card(cardHeader, cardBody);
            var form = await html.FormAsync("post", card);

            output.Content.AppendLine("====DynamicFormStart====");
            output.Content.AppendHtml(form);
            output.Content.AppendLine("====DynamicFormEnd====");
        }

        private ModelExplorer GetItemModel()
        {
            var item = PageModel.ModelExplorer.GetProperty("Item");
            return item;
        }
    }
}
