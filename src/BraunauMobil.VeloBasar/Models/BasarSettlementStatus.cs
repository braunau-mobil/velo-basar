namespace BraunauMobil.VeloBasar.Models;

public record BasarSettlementStatus (
    bool HasSettlementStarted,
    SellerGroupSettlementStatus OverallStatus,
    SellerGroupSettlementStatus MustBeSettledOnSite,
    SellerGroupSettlementStatus MayBeSettledOnSite
);
