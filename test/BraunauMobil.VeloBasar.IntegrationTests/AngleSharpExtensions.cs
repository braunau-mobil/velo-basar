using AngleSharp.Dom;

namespace BraunauMobil.VeloBasar.IntegrationTests;

public static class AngleSharpExtensions
{
    public static IHtmlAnchorElement QueryAnchorByText(this IParentNode node, string text)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(text);

        return node.QueryElement<IHtmlAnchorElement>("a", button => button.InnerHtml == text);
    }
    
    public static IHtmlButtonElement QueryButtonByText(this IParentNode node, string text)

    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(text);

        return node.QueryElement<IHtmlButtonElement>("button", button => button.InnerHtml == text);
    }

    public static IHtmlAnchorElement QueryTableDetailsLink(this IParentNode node, int rowId)
    {
        ArgumentNullException.ThrowIfNull(node);

        IHtmlTableElement? table = node.QuerySelector<IHtmlTableElement>("table");
        table.Should().NotBeNull();
        IHtmlTableRowElement headerRow = table!.Rows[0];
        INode idHeaderCell = headerRow.Cells.Should().ContainSingle(cell => cell.TextContent == "Id").Subject;
        int idColumnIndex = headerRow.Cells.Index(idHeaderCell);

        IHtmlTableRowElement row = table.Rows.Should().ContainSingle(row => row.Cells[idColumnIndex].TextContent == rowId.ToString()).Subject;
        return row.QueryAnchorByText("Details");
    }

    public static IHtmlFormElement QueryForm(this IParentNode node, string selector = "form")
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(selector);

        return node.QuerySelector(selector)
            .Should().BeAssignableTo<IHtmlFormElement>().Subject;
    }

    public static TElement QueryElement<TElement>(this IParentNode node, string selector)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(selector);

        return node.QuerySelector(selector)
            .Should().BeAssignableTo<TElement>().Subject;
    }

    public static IHtmlTableElement QueryTable(this IParentNode node)
    {
        ArgumentNullException.ThrowIfNull(node);

        return node.QueryElement<IHtmlTableElement>("table");
    }

    public static TElement QueryElement<TElement>(this IParentNode node, string selector, Func<TElement, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(selector);

        TElement? element = node.QuerySelectorAll(selector)
            .OfType<TElement>()
            .FirstOrDefault(predicate);
        element.Should().NotBeNull();
        return element!;
    }
}
