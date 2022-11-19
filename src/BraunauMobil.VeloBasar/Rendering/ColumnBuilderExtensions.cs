using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Xan.AspNetCore.Rendering;

namespace BraunauMobil.VeloBasar.Rendering;

public static class ColumnBuilderExtensions
{
    public static TableBuilder<TItem> ForLink<TItem>(this ColumnBuilder<TItem> builder, Func<TItem, string> getUrl, LocalizedString text)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(getUrl);
        ArgumentNullException.ThrowIfNull(text);

        return builder
            .ForLink(getUrl, item => text.ToHtml());
    }

    public static TableBuilder<TItem> ForLink<TItem>(this ColumnBuilder<TItem> builder, Func<TItem, string> getUrl, Func<TItem, IHtmlContent> getContent)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(getUrl);
        ArgumentNullException.ThrowIfNull(getContent);

        return builder
            .ForLink(getUrl, getContent, item => true);
    }

    public static TableBuilder<TItem> ForLink<TItem>(this ColumnBuilder<TItem> builder, Func<TItem, string> getUrl, LocalizedString text, Func<TItem, bool> isVisible)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(getUrl);
        ArgumentNullException.ThrowIfNull(text);
        ArgumentNullException.ThrowIfNull(isVisible);

        return builder
            .ForLink(getUrl, item => text.ToHtml(), isVisible);
    }

    public static TableBuilder<TItem> ForLink<TItem>(this ColumnBuilder<TItem> builder, Func<TItem, string> getUrl, Func<TItem, IHtmlContent> getContent, Func<TItem, bool> isVisible)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(getUrl);
        ArgumentNullException.ThrowIfNull(getContent);
        ArgumentNullException.ThrowIfNull(isVisible);

        return builder
            .For(item =>
            {
                if (isVisible(item))
                {
                    string url = getUrl(item);
                    TagBuilder link = builder.Html.Link(url);
                    link.InnerHtml.SetHtmlContent(getContent(item));
                    return link;
                }
                else
                {
                    return new HtmlString(string.Empty);
                }
            });
    }
}
