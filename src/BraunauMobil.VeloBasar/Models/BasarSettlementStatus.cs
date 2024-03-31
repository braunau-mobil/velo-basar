namespace BraunauMobil.VeloBasar.Models;

public record BasarSettlementStatus (
    bool HasSettlementStarted,
    int OverallNotSettledCount,
    int OnSiteCount,
    int RemoteCount
);
