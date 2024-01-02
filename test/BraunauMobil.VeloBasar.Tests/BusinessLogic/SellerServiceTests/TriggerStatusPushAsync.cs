namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SellerServiceTests;

public class TriggerStatusPushAsync
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [VeloAutoData]
    public async void CallsStatusPushService(int basarId, int sellerId)
    {
        //  Arrange
        A.CallTo(() => StatusPushService.PushSellerAsync(basarId, sellerId)).DoesNothing();

        //  Act
        await Sut.TriggerStatusPushAsync(basarId, sellerId);

        //  Assert
        A.CallTo(() => StatusPushService.PushSellerAsync(basarId, sellerId)).MustHaveHappenedOnceExactly();
    }
}
