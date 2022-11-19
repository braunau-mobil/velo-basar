using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Parameters;

public sealed class ProductListParameter
    : ListParameter
{
    public ProductListParameter()
    { }

    public ProductListParameter(ProductListParameter other)
        : base(other)
    {
        ArgumentNullException.ThrowIfNull(other);

        StorageState = other.StorageState;
        ValueState = other.ValueState;
        BrandId = other.BrandId;
        ProductTypeId = other.ProductTypeId;
    }

    public StorageState? StorageState { get; set; }

    public ValueState? ValueState { get; set; }

    public int? BrandId { get; set; }

    public int? ProductTypeId { get; set; }

    protected override ListParameter Clone()
        => new ProductListParameter(this);
}
