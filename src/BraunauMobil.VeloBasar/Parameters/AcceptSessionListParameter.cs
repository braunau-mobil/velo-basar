using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Parameters;

public sealed class AcceptSessionListParameter
    : ListParameter
    , IHasBasarId
{
    public AcceptSessionListParameter()
    { }

    public AcceptSessionListParameter(AcceptSessionListParameter other)
        : base(other)
    {
        ArgumentNullException.ThrowIfNull(other);

        AcceptSessionState = other.AcceptSessionState;
        BasarId = other.BasarId;
    }

    public AcceptSessionState? AcceptSessionState { get; set; }
    
    public int BasarId { get; set; }

    protected override ListParameter Clone()
        => new AcceptSessionListParameter(this);
}
