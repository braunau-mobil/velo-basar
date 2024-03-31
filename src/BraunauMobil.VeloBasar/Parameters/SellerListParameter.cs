using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Parameters;

public sealed class SellerListParameter
    : ListParameter
    , IHasBasarId
{
    public SellerListParameter()
    {
        State = null;
    }

    public SellerListParameter(SellerListParameter other)
        : base(other)
    {
        ArgumentNullException.ThrowIfNull(other);

        BasarId = other.BasarId;
        SettlementType = other.SettlementType;
        ValueState = other.ValueState;
    }

    public int BasarId { get; set; }

    public SellerSettlementType? SettlementType { get; set; }

    public ValueState? ValueState { get; set; }

    protected override ListParameter Clone()
        => new SellerListParameter(this);
}
