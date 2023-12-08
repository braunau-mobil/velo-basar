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
        Brand = other.Brand;
        ProductTypeId = other.ProductTypeId;
    }

    public StorageState? StorageState { get; set; }

    public ValueState? ValueState { get; set; }

    public string? Brand { get; set; }

    public int? ProductTypeId { get; set; }

    protected override ListParameter Clone()
        => new ProductListParameter(this);
}
