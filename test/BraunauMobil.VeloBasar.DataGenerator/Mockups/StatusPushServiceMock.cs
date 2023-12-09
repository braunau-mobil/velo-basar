using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.DataGenerator.Mockups;

public class StatusPushServiceMock
    : IStatusPushService
{
    public bool IsEnabled => true;

    public async Task PushSellerAsync(int basarId, int sellerId)
    {
        await Task.CompletedTask;
    }
}
