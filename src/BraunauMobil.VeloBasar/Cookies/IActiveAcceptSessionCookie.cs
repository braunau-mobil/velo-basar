namespace BraunauMobil.VeloBasar.Cookies;

public interface IActiveAcceptSessionCookie
    : ICookie
{
    void ClearActiveAcceptSession();

    int? GetActiveAcceptSessionId();

    void SetActiveAcceptSession(AcceptSessionEntity session);
}
