namespace BraunauMobil.VeloBasar.Models;

public record BasarSettlementStatus (
    bool HasAny,
    SellerGroupSettlementStatus OverallStatus,
    SellerGroupSettlementStatus MustBeSettledOnSite,
    SellerGroupSettlementStatus MayBeSettledOnSite
);
