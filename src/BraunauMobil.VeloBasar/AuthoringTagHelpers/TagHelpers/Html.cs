using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Models.Interfaces;
using BraunauMobil.VeloBasar.Pages.Generic;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
    public class Html
    {
        private static readonly int[] _pageSizes = new [] { 5, 10, 15, int.MaxValue };

        private readonly IHtmlLocalizer<SharedResource> _localizer;
        private readonly IHtmlGenerator _htmlGenerator;
        private readonly HtmlEncoder _htmlEncoder;
        private readonly ViewContext _viewContext;

        public Html(IHtmlLocalizer<SharedResource> htmlLocalizer, IHtmlGenerator htmlGenerator, HtmlEncoder htmlEncoder, ViewContext viewContext)
        {
            _localizer = htmlLocalizer;
            _htmlGenerator = htmlGenerator;
            _htmlEncoder = htmlEncoder;
            _viewContext = viewContext;
        }

        public async Task<IHtmlContent> CancelButtonAsync(VeloPage page) => await LinkSecondaryAsync(page, _localizer["Abbrechen"]);
        public TagBuilder Card(IHtmlContent header, IHtmlContent body)
        {
            var card = Div("card my-3");
            card.InnerHtml.AppendHtml(header);
            card.InnerHtml.AppendHtml(body);
            return card;
        }
        public TagBuilder CardBody(string additionalCssClasses, params IHtmlContent[] children)
        {
            if (children == null) throw new ArgumentNullException(nameof(children));

            var cardBody = Div($"card-body {additionalCssClasses}");
            foreach (var child in children)
            {
                cardBody.InnerHtml.AppendHtml(child);
            }
            return cardBody;
        }
        public TagBuilder CardHeader(params IHtmlContent[] children)
        {
            if (children == null) throw new ArgumentNullException(nameof(children));

            var cardHeader = Div("card-header d-flex justify-content-between align-items-md-center");
            foreach (var child in children)
            {
                cardHeader.InnerHtml.AppendHtml(child);
            }
            return cardHeader;
        }
        public TagBuilder CardHeaderTitle(IHtmlContent title)
        {
            var div = Div();
            div.InnerHtml.AppendHtml(H5(title));
            return div;
        }
        public TagBuilder Div(string cssClass = null) => Tag("div", cssClass);
        public TagBuilder EditCardBody(params IHtmlContent[] children) => CardBody("", children);
        public async Task<IHtmlContent> FormAsync(string method, params IHtmlContent[] children)
        {
            if (children == null) throw new ArgumentNullException(nameof(children));

            return await FormAsync(method, children, Array.Empty<ITagHelper>());
        }
        public async Task<IHtmlContent> FormAsync(VeloPage veloPage, string method, params IHtmlContent[] children)
        {
            if (children == null) throw new ArgumentNullException(nameof(children));

            var tagHelpers = new[] { CreateTagHelper(veloPage) };

            return await FormAsync(method, children, tagHelpers);
        }
        public async Task<IHtmlContent> FormRowAsync(ModelExplorer property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            if (property.Metadata.HasHiddenInputAttribute())
            {
                return await HiddenInputAsync(property);
            }
            return await FormGroupRowAsync(property);
        }
        public async Task<IHtmlContent> GetGeneratedContentFromTagHelpersAsync(string tagName, TagMode tagMode, IEnumerable<ITagHelper> tagHelpers, TagHelperAttributeList attributes)
        {
            var output = await GetOutputFromTagHelpersAsync(tagName, tagMode, tagHelpers, attributes);
            return RenderTag(output);
        }
        public async Task<IHtmlContent> GetGeneratedContentFromTagHelperAsync(string tagName, TagMode tagMode, ITagHelper tagHelper, TagHelperAttributeList attributes) => await GetGeneratedContentFromTagHelpersAsync(tagName, tagMode, new[] { tagHelper }, attributes);
        public async Task<TagHelperOutput> GetOutputFromTagHelpersAsync(string tagName, TagMode tagMode, IEnumerable<ITagHelper> tagHelpers, TagHelperAttributeList attributes)
        {
            if (tagHelpers == null) throw new ArgumentNullException(nameof(tagHelpers));

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

            return output;
        }
        public async Task<TagHelperOutput> GetOutputFromTagHelperAsync(string tagName, TagMode tagMode, ITagHelper tagHelper, TagHelperAttributeList attributes) => await GetOutputFromTagHelpersAsync(tagName, tagMode, new[] { tagHelper }, attributes);
        public TagBuilder H5(IHtmlContent text)
        {
            var h5 = new TagBuilder("h5");
            h5.AddCssClass("mx-2");
            h5.InnerHtml.AppendHtml(text);
            return h5;
        }
        public TagBuilder Li(string cssClass = null) => Tag("li", cssClass);
        public async Task<TagBuilder> LinkAsync(VeloPage page, IHtmlContent content) => await LinkAsync(page, content, new TagHelperAttributeList());
        public async Task<TagBuilder> LinkAsync(VeloPage page, IHtmlContent content, TagHelperAttributeList attributes)
        {
            var div = Div();

            var veloPageLink = await GetOutputFromTagHelperAsync("a", TagMode.StartTagAndEndTag, CreateTagHelper(page), attributes);
            veloPageLink.Content.SetHtmlContent(content);
            div.InnerHtml.SetHtmlContent(RenderTag(veloPageLink));

            return div;
        }
        public async Task<TagBuilder> LinkPrimaryAsync(VeloPage page, IHtmlContent content)
        {
            var attributes = new TagHelperAttributeList
            {
                {  "class", "ml-auto btn btn-primary" }
            };

            return await LinkAsync(page, content, attributes);
        }
        public async Task<TagBuilder> LinkSecondaryAsync(VeloPage page, IHtmlContent content)
        {
            var attributes = new TagHelperAttributeList
            {
                {  "class", "ml-auto btn btn-secondary" }
            };

            return await LinkAsync(page, content, attributes);
        }
        public TagBuilder Nav(string cssClass = null) => Tag("nav", cssClass);
        public async Task<IHtmlContent> PaginationAsync(IPaginatable paginatable)
        {
            if (paginatable == null) throw new ArgumentNullException(nameof(paginatable));

            var row = Div("row");
            var pageSizesNav = Nav();
            var pagesSizesUl = Ul("pagination");
            
            foreach (var pageSize in _pageSizes)
            {
                var isActive = paginatable.PageSize == pageSize ? "active" : "";
                var linkText = pageSize == int.MaxValue ? "Alle" : $"{pageSize}";

                var pageSizeLi = await PaginationItemAsync(paginatable.GetPaginationPage(paginatable.PageIndex, pageSize), _localizer[linkText], isActive);
                pagesSizesUl.InnerHtml.AppendHtml(pageSizeLi);
            }

            pageSizesNav.InnerHtml.AppendHtml(pagesSizesUl);
            row.InnerHtml.AppendHtml(pageSizesNav);

            if (paginatable.TotalPages > 1)
            {
                var pagesUl = Ul("pagination flex-wrap");

                var prevDisabled = !paginatable.HasPreviousPage ? "disabled" : "";
                var prevLi = await PaginationItemAsync(paginatable.GetPaginationPage(paginatable.PageIndex - 1), _localizer["<<<"], prevDisabled);
                pagesUl.InnerHtml.AppendHtml(prevLi);

                for (var pageNumber = 1; pageNumber <= paginatable.TotalPages; pageNumber++)
                {
                    var isActive = pageNumber == paginatable.PageIndex ? "active" : "";
                    var pageLi = await PaginationItemAsync(paginatable.GetPaginationPage(pageNumber), new StringHtmlContent($"{pageNumber}"), isActive);
                    pagesUl.InnerHtml.AppendHtml(pageLi);
                }

                var nextDisabled = !paginatable.HasNextPage ? "disabled" : "";
                var nextLi = await PaginationItemAsync(paginatable.GetPaginationPage(paginatable.PageIndex + 1), _localizer[">>>"], nextDisabled);
                pagesUl.InnerHtml.AppendHtml(nextLi);
                
                var pagesNav = Nav("mx-4");
                pagesNav.InnerHtml.AppendHtml(pagesUl);
                
                row.InnerHtml.AppendHtml(pagesNav);
            }

            return row;
        }
        public IHtmlContent RenderTag(IHtmlContent output)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));

            using var writer = new StringWriter();
            output.WriteTo(writer, _htmlEncoder);
            return new HtmlString(writer.ToString());
        }
        public async Task<IHtmlContent> SaveButtonAsync(VeloPage page)
        {
            var tagHelpers = new List<ITagHelper>
            {
                new VeloPageTagHelper(_htmlGenerator)
                {
                    Page = page,
                    ViewContext = _viewContext
                }
            };
            var attributes = new TagHelperAttributeList
            {
                 new TagHelperAttribute("class", "btn btn-primary mx-1"),
                 new TagHelperAttribute("type", "submit"),
                 new TagHelperAttribute("value", _localizer["Speichern"].Value)
            };
            return await GetGeneratedContentFromTagHelpersAsync("input", TagMode.SelfClosing, tagHelpers, attributes);
        }
        public async Task<IHtmlContent> SearchAsync(ISearchable searchable)
        {
            if (searchable == null) throw new ArgumentNullException(nameof(searchable));

            var div = Div("d-flex form-actions no-color align-items-md-center");

            var title = Div("mx-2");
            title.InnerHtml.SetHtmlContent(_localizer["Suchen:"]);
            div.InnerHtml.AppendHtml(title);

            var input = Input("mx-2 form-control", "text", searchable.SearchString, "searchString");
            div.InnerHtml.AppendHtml(input);

            var searchButton = Input("btn btn-secondary mx-2", "submit", _localizer["Suchen"].Value);
            div.InnerHtml.AppendHtml(searchButton);

            var resetButton = await LinkAsync(searchable.GetResetPage(), _localizer["Zurücksetzen"]);
            div.InnerHtml.AppendHtml(resetButton);

            return div;
        }
        public TagBuilder Table(IHtmlContent header, IHtmlContent body)
        {
            var table = new TagBuilder("table");
            table.AddCssClass("table table-striped m-0");

            table.InnerHtml.AppendHtml(header);
            table.InnerHtml.AppendHtml(body);

            return table;
        }
        public async Task<IHtmlContent> TableBodyAsync(DynamicTableConfiguration configuration, IListPageModel listPageModel)
        {
            var tbody = TBody();

            foreach (var item in listPageModel.Items)
            {
                var tr = Tr();

                foreach (var column in configuration.Columns)
                {
                    var cell = ItemCell(item, column.Property);
                    tr.InnerHtml.AppendHtml(cell);
                }

                tr.InnerHtml.AppendHtml(await EditCellAsync(listPageModel, item));
                tr.InnerHtml.AppendHtml(await DeleteCellAsync(listPageModel, item));

                tbody.InnerHtml.AppendHtml(tr);
            }

            return tbody;
        }
        public TagBuilder TableCardBody(params IHtmlContent[] children) => CardBody("p-0", children);
        public IHtmlContent TableHeader(DynamicTableConfiguration configuration)
        {
            var thead = THead();
            var tr = Tr();

            foreach (var column in configuration.Columns)
            {
                var th = Th($"width: {column.Width};", "col");
                th.InnerHtml.SetContent(column.Property.DisplayName ?? column.Property.Name);
                tr.InnerHtml.AppendHtml(th);
            }

            tr.InnerHtml.AppendHtml(Th("width: auto;", "col"));
            tr.InnerHtml.AppendHtml(Th("width: auto;", "col"));

            thead.InnerHtml.AppendHtml(tr);
            return thead;
        }
        public TagBuilder TBody() => Tag("tbody");
        public TagBuilder Td() => Tag("td");
        public TagBuilder Th(string style, string scope)
        {
            var tr = new TagBuilder("th");
            tr.MergeAttribute("style", style);
            tr.MergeAttribute("scope", scope);
            return tr;
        }
        public TagBuilder THead() => Tag("thead");
        public TagBuilder Tr() => Tag("tr");
        public TagBuilder Ul(string cssClass) => Tag("ul", cssClass);

        private ITagHelper CreateTagHelper(VeloPage page) => new VeloPageTagHelper(_htmlGenerator)
        {
            Page = page,
            ViewContext = _viewContext
        };
        private async Task<IHtmlContent> DeleteCellAsync(IListPageModel listPageModel, IListItem listItem)
        {
            var td = Td();

            if (await listPageModel.CanDeleteAsync(listItem))
            {
                td.InnerHtml.AppendHtml(await LinkAsync(listPageModel.DeletePage(listItem), _localizer["Löschen"]));
            }
            else if (listItem.Item is IStateModel stateModel)
            {
                var text = "Deaktivieren";
                var stateToSet = ObjectState.Disabled;
                if (stateModel.State == ObjectState.Disabled)
                {
                    stateToSet = ObjectState.Enabled;
                    text = "Aktivieren";
                }
                td.InnerHtml.AppendHtml(await LinkAsync(listPageModel.SetStatePage(listItem, stateToSet), _localizer[text]));
            }

            return td;
        }
        private async Task<IHtmlContent> EditCellAsync(IListPageModel listPageModel, IListItem listItem)
        {
            var td = Td();

            td.InnerHtml.AppendHtml(await LinkAsync(listPageModel.EditPage(listItem), _localizer["Bearbeiten"]));

            return td;
        }
        private async Task<IHtmlContent> EnumSelectAsync(ModelExplorer property)
        {
            var tagHelpers = new[]
            {
                new SelectTagHelper(_htmlGenerator)
                {
                    For = new ModelExpression(property.Metadata.PropertyName, property),
                    Items = property.Metadata.GetEnumSelectList(),
                    ViewContext = _viewContext
                }
            };

            var attributes = new TagHelperAttributeList
            {
                 new TagHelperAttribute("class", "col-10 form-control")
            };

            return await GetGeneratedContentFromTagHelpersAsync("select", TagMode.StartTagAndEndTag, tagHelpers, attributes);
        }
        private async Task<IHtmlContent> FormAsync(string method, IEnumerable<IHtmlContent> children, IEnumerable<ITagHelper> additionalTagHelpers)
        {
            var tagHelpers = new List<ITagHelper>(additionalTagHelpers)
            {
                new FormTagHelper(_htmlGenerator)
                {
                    Antiforgery = true,
                    ViewContext = _viewContext
                }
            };
            var attributes = new TagHelperAttributeList
            {
                { "method", method }
            };

            var form = await GetOutputFromTagHelpersAsync("form", TagMode.StartTagAndEndTag, tagHelpers, attributes);
            foreach (var child in children)
            {
                form.Content.AppendHtml(child);
            }
            return RenderTag(form);
        }
        private async Task<IHtmlContent> FormGroupRowAsync(ModelExplorer property)
        {
            var label = await FormLabelAsync(property);
            IHtmlContent control;
            if (property.Metadata.IsEnum)
            {
                control = await EnumSelectAsync(property);
            }
            else if (property.Metadata.HasMultilineTextDataTypeAttribute())
            {
                control = await TextAreaAsync(property);
            }
            else
            {
                control = await InputAsync(property);
            }
            return FormGroupRow(label, control);

        }
        private IHtmlContent FormGroupRow(IHtmlContent left, IHtmlContent right)
        {
            var div = Div("form-group row");
            div.InnerHtml.AppendHtml(left);
            div.InnerHtml.AppendHtml(right);
            return div;
        }
        private async Task<IHtmlContent> FormLabelAsync(ModelExplorer property)
        {
            var labelTagHelper = new LabelTagHelper(_htmlGenerator)
            {
                For = new ModelExpression(property.Metadata.PropertyName, property),
                ViewContext = _viewContext
            };

            var attributes = new TagHelperAttributeList
            {
                new TagHelperAttribute("class", "col-2 col-form-label")
            };

            return await GetGeneratedContentFromTagHelperAsync("label", TagMode.StartTagAndEndTag, labelTagHelper, attributes);
        }
        private IHtmlContent ItemCell(IListItem listItem, ModelMetadata property)
        {
            if (property.Name == "Id")
            {
                var th = Th("", "row");
                th.InnerHtml.SetContent(property.PropertyGetter(listItem.Item).ToString());
                return th;
            }

            var td = Td();
            var value = property.PropertyGetter(listItem.Item);
            if (value != null)
            {
                td.InnerHtml.SetContent(value.ToString());
            }
            return td;
        }
        private async Task<IHtmlContent> HiddenInputAsync(ModelExplorer property)
        {
            var attributes = new TagHelperAttributeList
            {
                new TagHelperAttribute("type", "hidden")
            };

            return await InputAsync(property, attributes);
        }
        private IHtmlContent Input(string cssClass, string type, string value, string name = null)
        {
            var input = new TagBuilder("input");
            input.AddCssClass(cssClass);
            input.MergeAttribute("type", type);
            input.MergeAttribute("value", value);
            if (name != null)
            {
                input.MergeAttribute("name", name);
            }
            return input;
        }
        private async Task<IHtmlContent> InputAsync(ModelExplorer property)
        {
            var attributes = new TagHelperAttributeList
            {
                 new TagHelperAttribute("class", "col-10 form-control")
            };
            return await InputAsync(property, attributes, true);
        }
        private async Task<IHtmlContent> InputAsync(ModelExplorer property, TagHelperAttributeList attributes, bool addValidation = false)
        {
            var tagHelpers = new List<ITagHelper>
            {
                new InputTagHelper(_htmlGenerator)
                {
                    For = new ModelExpression(property.Metadata.PropertyName, property),
                    ViewContext = _viewContext
                },
            };

            if (addValidation)
            {
                tagHelpers.Add(new ValidationTagHelper()
                {
                    For = new ModelExpression(property.Metadata.PropertyName, property),
                    ViewContext = _viewContext
                });
            }

            return await GetGeneratedContentFromTagHelpersAsync("input", TagMode.SelfClosing, tagHelpers, attributes);
        }
        private async Task<IHtmlContent> PaginationItemAsync(VeloPage page, IHtmlContent content, string status = null)
        {
            var linkAttributes = new TagHelperAttributeList
            {
                { "class", "page-link" }
            };
            var pageLink = await LinkAsync(page, content, linkAttributes);

            var li = Li($"page-item {status ?? ""}");
            li.InnerHtml.AppendHtml(pageLink);
            return li;
        }
        private TagBuilder Tag(string name, string cssClass = null)
        {
            var tag = new TagBuilder(name);
            if (cssClass != null)
            {
                tag.AddCssClass(cssClass);
            }
            return tag;
        }
        private async Task<IHtmlContent> TextAreaAsync(ModelExplorer property)
        {
            var attributes = new TagHelperAttributeList
            {
                 new TagHelperAttribute("class", "col-10 form-control")
            };
            var tagHelpers = new List<ITagHelper>
            {
                new TextAreaTagHelper(_htmlGenerator)
                {
                    For = new ModelExpression(property.Metadata.PropertyName, property),
                    ViewContext = _viewContext
                },
            };

            tagHelpers.Add(new ValidationTagHelper()
            {
                For = new ModelExpression(property.Metadata.PropertyName, property),
                ViewContext = _viewContext
            });

            return await GetGeneratedContentFromTagHelpersAsync("textarea", TagMode.StartTagAndEndTag, tagHelpers, attributes);
        }
    }
}
