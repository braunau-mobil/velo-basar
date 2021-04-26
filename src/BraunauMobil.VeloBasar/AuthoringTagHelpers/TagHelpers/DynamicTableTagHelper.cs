using BraunauMobil.VeloBasar.Pages.Generic;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
    public class DynamicTableTagHelper : TagHelper
    {
        public ModelExpression PageModel { get; set; }
        public IHtmlContent NewItemText { get; set; }
        public IHtmlContent TitleText { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlLocalizer<SharedResource> _localizer;
        private readonly IHtmlGenerator _htmlGenerator;
        private readonly HtmlEncoder _htmlEncoder;

        public DynamicTableTagHelper(IHtmlGenerator htmlGenerator, HtmlEncoder htmlEncoder, IHtmlLocalizer<SharedResource> localizer)
        {
            _htmlGenerator = htmlGenerator;
            _localizer = localizer;
            _htmlEncoder = htmlEncoder;
        }

        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (output == null) throw new ArgumentNullException(nameof(output));

            var html = new Html(_localizer, _htmlGenerator, _htmlEncoder, ViewContext);
            var listPageModel = PageModel.Model as IListPageModel;
            if (listPageModel == null)
            {
                throw new InvalidOperationException("@TODO");
            }

            var configuration = new DynamicTableConfiguration(GetItemModel());
            context.Items[nameof(DynamicTableConfiguration)] = configuration;
            await output.GetChildContentAsync();

            if (configuration.Columns.Count == 0)
            {
                foreach (var property in PageModel.ModelExplorer.Properties)
                {
                    configuration.Columns.Add(new DynamicPropertyColumnConfiguration { Property = property.Metadata});
                }
            }

            output.Content.AppendLine("====DynamicTableStart====");

            var headerItems = new List<IHtmlContent>
            {
                html.CardHeaderTitle(TitleText)
            };
            if (listPageModel is ISearchable searchable)
            {
                headerItems.Add(await html.SearchAsync(searchable));
            }
            headerItems.Add(await html.LinkPrimaryAsync(listPageModel.CreatePage(), NewItemText));

            var cardHeader = html.CardHeader(headerItems.ToArray());
            var table = html.Table(html.TableHeader(configuration), await html.TableBodyAsync(configuration, listPageModel));
            var cardBody = html.TableCardBody(table);
            var card = html.Card(cardHeader, cardBody);

            var form = await html.FormAsync(listPageModel.SearchPage(), "get", card);
            output.Content.AppendHtml(form);
            output.Content.AppendHtml(await html.PaginationAsync(listPageModel.Paginatable));

            output.Content.AppendLine("====DynamicTableEnd====");
        }

        private ModelMetadata GetItemModel()
        {
            var items = PageModel.ModelExplorer.GetProperty("Items");
            var list = items.GetProperty("List");
            if (list.Metadata.IsEnumerableType)
            {
                var itemViewModel = list.Metadata.ElementMetadata;
                return itemViewModel.GetProperty("Item");
            }

            throw new InvalidOperationException("todo");
        }
    }
}
