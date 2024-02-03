using System.Threading;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.WordPressStatusPushServiceTests;

public class PushSellerAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ShouldEnqueueBackgroundWorkItem(int basarId, SellerEntity seller)
    {
        //  Arrange
        Db.Sellers.Add(seller);
        await Db.SaveChangesAsync();

        A.CallTo(() => BackgroundTaskQueue.QueueBackgroundWorkItem(A<Func<CancellationToken, Task>>.Ignored)).DoesNothing();

        //  Act
        await Sut.PushSellerAsync(basarId, seller.Id);

        //  Assert
        A.CallTo(() => BackgroundTaskQueue.QueueBackgroundWorkItem(A<Func<CancellationToken, Task>>.Ignored)).MustHaveHappenedOnceExactly();
    }
}
