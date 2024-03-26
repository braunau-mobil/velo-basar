namespace BraunauMobil.VeloBasar.Configuration;

public sealed record PriceRange(decimal? From = null, decimal? To = null)
{
    public bool IsInRange(decimal value)
    {
        decimal from = From ?? decimal.MinValue;
        decimal to = To ?? decimal.MaxValue;
        return value >= from && value <= to;
    }

    public string GetLabel(IFormatProvider formatProvider)
    {
        ArgumentNullException.ThrowIfNull(formatProvider);

        if (From.HasValue && To.HasValue)
        {
            return string.Format(formatProvider, "{0:C} - {1:C}", From.Value, To.Value);
        }
        if (From.HasValue)
        {
            return string.Format(formatProvider, "{0:C}+", From.Value);
        }
        if (To.HasValue)
        {
            return string.Format(formatProvider, "-{0:C}", To.Value);
        }
        return string.Empty;
    }
}
