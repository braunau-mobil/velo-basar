using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Parameters;

public sealed class TransactionListParameter
    : ListParameter
{
    public TransactionListParameter()
    { }

    public TransactionListParameter(TransactionListParameter other)
        : base(other)
    {
        ArgumentNullException.ThrowIfNull(other);

        TransactionType = other.TransactionType;
    }

    public TransactionType? TransactionType { get; set; }

    protected override ListParameter Clone()
        => new TransactionListParameter(this);
}
