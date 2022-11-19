namespace BraunauMobil.VeloBasar.Models;

#nullable disable warnings
public sealed class AcceptProductModel
{
    public bool CanAccept { get; set; }

    public ProductEntity Entity { get; set; }

    public int SellerId { get; set; }

    public int SessionId { get; set; }

    public IReadOnlyList<ProductEntity> Products { get; set; }
}
