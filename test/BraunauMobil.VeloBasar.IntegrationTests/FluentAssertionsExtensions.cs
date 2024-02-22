using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.Text;

namespace BraunauMobil.VeloBasar.IntegrationTests;

public static class FluentAssertionsExtensions
{
    public static IHtmlTableElementAssertions Should(this IHtmlTableElement instance)
        => new(instance);
}

public class IHtmlTableElementAssertions(IHtmlTableElement instance)
    : ReferenceTypeAssertions<IHtmlTableElement, IHtmlTableElementAssertions>(instance)
{
    protected override string Identifier => "IHtmlTableElement";

    public AndConstraint<IHtmlTableElementAssertions> BeEquivalentTo(params object[][] rows)
    {
        ArgumentNullException.ThrowIfNull(rows);

        StringBuilder sb = new();
        if (Subject.Rows.Length != rows.Length)
        {
            sb.AppendLine($"Expected table to have {rows.Length} rows but actual: {Subject.Rows.Length}");
        }
        else
        {
            for (int row = 0; row < Subject.Rows.Length; row++)
            {
                if (Subject.Rows[row].Cells.Length != rows[row].Length)
                {
                    sb.AppendLine($"Expected table row {row} to have {rows[row].Length} cells but actual: {Subject.Rows[row].Cells.Length}");
                }
                else
                {
                    IHtmlTableRowElement rowElement = Subject.Rows[row];
                    for (int cell = 0; cell < rowElement.Cells.Length; cell++)
                    {
                        string expectedTextContent = ToTextContent(rows[row][cell]);
                        string actualCellContent = rowElement.Cells[cell].TextContent;

                        if (expectedTextContent != actualCellContent)
                        {
                            sb.AppendLine($"Expected table cell {row} in row {cell} to be \"{expectedTextContent}\" but actual: \"{actualCellContent}\"");
                        }
                    }
                }
            }
        }

        Execute.Assertion
            .ForCondition(sb.Length == 0)
            .FailWith(sb.ToString());

        return new(this);
    }

    public static string ToTextContent(object value)
    {
        if (value is string stringValue)
        {
            return stringValue;
        }

        string? toString = value.ToString();
        if (toString is null)
        {
            throw new ArgumentException("Null values are not allowed");
        }
        return toString;
    }
}
