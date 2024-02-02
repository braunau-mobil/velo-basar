namespace BraunauMobil.VeloBasar.Models.Documents;

public record ProductLabelDocumentModel(
    string Title,
    string BrandTypeInfo,
    string Description,
    string? Color,
    string? FrameNumber,
    string? TireSize,
    string Barcode,
    string Price
);
