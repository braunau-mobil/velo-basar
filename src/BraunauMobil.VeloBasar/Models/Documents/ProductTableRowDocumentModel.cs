namespace BraunauMobil.VeloBasar.Models.Documents;

public record ProductTableRowDocumentModel(
    string Id,
    string InfoText,
    string TireSize,
    string Price,
    string? SellerInfo
);
