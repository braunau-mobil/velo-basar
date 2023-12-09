namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SellerServiceTests;

public class TriggerStatusPushAsync
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [AutoData]
    public async void CallsStatusPushService(int basarId, int sellerId)
    {
        //  Arrange

        //  Act
        await Sut.TriggerStatusPushAsync(basarId, sellerId);

        //  Assert
        StatusPushService.Verify(_ => _.PushSellerAsync(basarId, sellerId), Times.Once);

        VerifyNoOtherCalls();
    }
}
