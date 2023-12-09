namespace BraunauMobil.VeloBasar.Models;

public record SellerGroupSettlementStatus(
    int TotalCount,
    int SettledCount
)
{
    public int Percentage
    {
        get
        {
            if (TotalCount == 0)
            {
                return 0;
            }

            return (int)((double)SettledCount / TotalCount * 100.0);
        }
    }
}

