namespace BraunauMobil.VeloBasar.Models;

public record BasarSettlementStatus (
    bool HasSettlementStarted,
    int OverallSettledCount,
    int OverallNotSettledCount,
    int MustComeBy,
    int MayComeBy
);
