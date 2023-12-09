namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface IStatusPushService
{
    bool IsEnabled { get; }

    Task PushSellerAsync(int basarId, int sellerId);
}
