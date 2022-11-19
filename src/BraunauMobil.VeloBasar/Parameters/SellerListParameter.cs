using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Parameters;

public sealed class SellerListParameter
    : ListParameter
{
    public SellerListParameter()
    {
        State = null;
    }

    public SellerListParameter(SellerListParameter other)
        : base(other)
    {
        ArgumentNullException.ThrowIfNull(other);

        ValueState = other.ValueState;
    }

    public ValueState? ValueState { get; set; }

    protected override ListParameter Clone()
        => new SellerListParameter(this);
}
