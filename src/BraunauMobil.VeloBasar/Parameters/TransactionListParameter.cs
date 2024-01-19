using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Parameters;

public sealed class TransactionListParameter
    : ListParameter
    , IHasBasarId
{
    public TransactionListParameter()
    { }

    public TransactionListParameter(TransactionListParameter other)
        : base(other)
    {
        ArgumentNullException.ThrowIfNull(other);

        BasarId = other.BasarId;
        TransactionType = other.TransactionType;
    }

    public int BasarId { get; set; }

    public TransactionType? TransactionType { get; set; }

    protected override ListParameter Clone()
        => new TransactionListParameter(this);
}
