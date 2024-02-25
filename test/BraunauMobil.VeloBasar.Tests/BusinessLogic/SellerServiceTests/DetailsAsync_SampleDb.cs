namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SellerServiceTests;

public class DetailsAsync_SampleDb
    : TestBase
{
    private readonly VeloFixture _fixture = new();

    [Theory]
    [VeloAutoData]
    public async Task DetailsAreCorrect(BasarEntity basar, SellerEntity seller, int transactionCount)
    {
        //  Arrange
        basar.ProductCommissionPercentage = 10;
        Db.Basars.Add(basar);
        seller.ValueState = ValueState.Settled;
        Db.Sellers.Add(seller);
        CreateProduct(basar, seller, StorageState.Available, ValueState.NotSettled);
        CreateProduct(basar, seller, StorageState.Available, ValueState.NotSettled);
        CreateProduct(basar, seller, StorageState.Available, ValueState.Settled);
        CreateProduct(basar, seller, StorageState.Available, ValueState.Settled);
        CreateProduct(basar, seller, StorageState.Available, ValueState.Settled);
        CreateProduct(basar, seller, StorageState.Sold, ValueState.NotSettled);
        CreateProduct(basar, seller, StorageState.Sold, ValueState.NotSettled);
        CreateProduct(basar, seller, StorageState.Sold, ValueState.Settled);
        Db.Transactions.AddRange(_fixture.BuildTransaction(basar, seller).CreateMany(transactionCount));
        await Db.SaveChangesAsync();

        A.CallTo(() => StatusPushService.IsEnabled).Returns(true);

        //  Act
        SellerDetailsModel model = await Sut.GetDetailsAsync(basar.Id, seller.Id);

        //  Assert
        using (new AssertionScope())
        {
            model.AcceptedProductCount.Should().Be(8);
            model.CanPushStatus.Should().BeTrue();
            model.Entity.Id.Should().Be(seller.Id);
            model.Entity.ValueState.Should().Be(ValueState.Settled);
            model.NotSoldProductCount.Should().Be(5);
            model.PickedUpProductCount.Should().Be(3);
            model.Products.Should().HaveCount(8);
            model.SettlementAmout.Should().Be(27M);
            model.SoldProductCount.Should().Be(3);
            model.Transactions.Should().HaveCount(transactionCount);
        }

        A.CallTo(() => StatusPushService.IsEnabled).MustHaveHappenedOnceExactly();
    }

    private void CreateProduct(BasarEntity basar, SellerEntity seller, StorageState storageState, ValueState valueState)
    {
        AcceptSessionEntity session = _fixture.BuildAcceptSession(basar)
            .With(session => session.Seller, seller)
            .Create();
        ProductEntity product = _fixture.BuildProduct()
            .With(product => product.Session, session)
            .With(product => product.ValueState, valueState)
            .With(product => product.StorageState, storageState)
            .With(product => product.Price, 10M)
            .Create();
        session.Products.Add(product);
        Db.AcceptSessions.Add(session);
    }
}
