namespace BraunauMobil.VeloBasar.Models.Documents;

public record ProductsTableDocumentModel(
    string IdColumnTitle,
    string ProductDescriptionColumnTitle,
    string SizeColumnTitle,
    string PriceColumnTitle,
    string SumText,
    string CountText,
    string PriceText,
    string? SellerInfoText,
    IReadOnlyCollection<ProductTableRowDocumentModel> Rows
);
