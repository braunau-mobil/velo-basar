namespace BraunauMobil.VeloBasar.Models;

public sealed class SellerCreateForAcceptanceModel
{
    public SellerCreateForAcceptanceModel(SellerEntity seller)
        : this(seller, false, Array.Empty<SellerEntity>())
    { }

    public SellerCreateForAcceptanceModel(SellerEntity seller, bool hasSearched, IReadOnlyList<SellerEntity> foundSellers)
    {
        Seller = seller ?? throw new ArgumentNullException(nameof(seller));
        HasSearched = hasSearched;
        FoundSellers = foundSellers ?? throw new ArgumentNullException(nameof(foundSellers));
    }

    public SellerEntity Seller { get; }

    public bool HasSearched { get; }

    public IReadOnlyList<SellerEntity> FoundSellers { get; }
}
