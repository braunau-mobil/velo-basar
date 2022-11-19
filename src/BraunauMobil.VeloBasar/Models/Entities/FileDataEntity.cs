using System.Diagnostics.CodeAnalysis;

namespace BraunauMobil.VeloBasar.Models.Entities;

#nullable disable warnings
public sealed class FileDataEntity
    : IEntity
{
    public const string PdfContentType = "application/pdf";

    public static FileDataEntity Pdf(string fileName, byte[] data)
    {
        ArgumentNullException.ThrowIfNull(fileName);
        ArgumentNullException.ThrowIfNull(data);
        return new()
        {
            ContentType = PdfContentType,
            Data = data ?? throw new ArgumentNullException(nameof(data)),
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName))
        };
    }

    public int Id { get; set; }

    public string ContentType { get; set; }

    public string FileName { get; set; }

    [SuppressMessage("Performance", "CA1819:Properties should not return arrays")]
    public byte[] Data { get; set; }
}
