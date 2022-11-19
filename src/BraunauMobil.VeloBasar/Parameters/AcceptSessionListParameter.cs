using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Parameters;

public sealed class AcceptSessionListParameter
    : ListParameter
{
    public AcceptSessionListParameter()
    { }

    public AcceptSessionListParameter(AcceptSessionListParameter other)
    {
        ArgumentNullException.ThrowIfNull(other);

        AcceptSessionState = other.AcceptSessionState;
    }

    public AcceptSessionState? AcceptSessionState { get; set; } = Models.AcceptSessionState.Uncompleted;

    protected override ListParameter Clone()
        => new AcceptSessionListParameter(this);
}
