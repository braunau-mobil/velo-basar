namespace BraunauMobil.VeloBasar.Configuration;

#nullable disable warnings
public class ExportSettings
{
    public string Delimiter { get; set; } = ";";

    public string EncodingName { get; set; } = "utf-8";

    public string NewLine { get; set; } = "\r\n";

    public char QuoteChar { get; set; } = '"';
}
