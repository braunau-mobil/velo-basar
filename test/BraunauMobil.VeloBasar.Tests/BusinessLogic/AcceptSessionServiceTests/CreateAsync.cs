namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptSessionServiceTests;

public class CreateAsync
	: TestBase
{
    [Theory]
    [AutoData]
    public async Task InvaliBasarAndSellerId_ThrowsInvalidOperationException(int basarId, int sellerId)
    {
        //  Arrange

        //  Act
        Func<Task> act = async () => await Sut.CreateAsync(basarId, sellerId);

        //  Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Theory]
	[AutoData]
	public async Task AcceptSessionIsCreatedAndInserted(BasarEntity basar, SellerEntity seller, DateTime dateTime)
	{
		//	Arrange
		Db.Basars.Add(basar);
		Db.Sellers.Add(seller);
		await Db.SaveChangesAsync();

		Clock.Setup(_ => _.GetCurrentDateTime())
			.Returns(dateTime);

		//	Act
		AcceptSessionEntity acceptSession = await Sut.CreateAsync(basar.Id, seller.Id);

		//	Assert
		acceptSession.Should().NotBeNull();
		acceptSession.StartTimeStamp.Should().Be(dateTime);
		acceptSession.Seller.Should().Be(seller);
		acceptSession.SellerId.Should().Be(seller.Id);
		acceptSession.Basar.Should().Be(basar);
		acceptSession.BasarId.Should().Be(basar.Id);

		VerifyNoOtherCalls();
	}
}
