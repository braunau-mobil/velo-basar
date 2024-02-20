namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetSettlementStatusAsync
    : TestBase
{
    private readonly VeloFixture _fixture = new();

    [Theory]
    [VeloAutoData]
    public async Task ShouldBeCorrect(BasarEntity basar)
    {
        //  Arrange
        Db.Basars.Add(basar);
        CreateSellerAndProduct(basar, "", ValueState.NotSettled, StorageState.Available);
        CreateSellerAndProduct(basar, "", ValueState.Settled, StorageState.Sold);
        CreateSellerAndProduct(basar, "DE76500105171978746253", ValueState.NotSettled, StorageState.Sold);
        CreateSellerAndProduct(basar, "AT363400012714178396", ValueState.Settled, StorageState.Available);
        await Db.SaveChangesAsync();

        //  Act
        BasarSettlementStatus result = await Sut.GetSettlementStatusAsync(basar.Id);

        //  Assert
        using (new AssertionScope())
        {
            result.HasAny.Should().BeTrue();
            
            result.MayBeSettledOnSite.SettledCount.Should().Be(1);
            result.MayBeSettledOnSite.TotalCount.Should().Be(2);
            
            result.MustBeSettledOnSite.SettledCount.Should().Be(1);
            result.MustBeSettledOnSite.TotalCount.Should().Be(2);

            result.OverallStatus.SettledCount.Should().Be(2);
            result.OverallStatus.TotalCount.Should().Be(4);
        }
    }

    private void CreateSellerAndProduct(BasarEntity basar, string iban, ValueState valueState, StorageState storageState)
    {
        SellerEntity seller = _fixture.BuildSeller()
            .With(seller => seller.IBAN, iban)
            .With(seller => seller.ValueState, valueState)
            .Create();
        AcceptSessionEntity session = _fixture.BuildAcceptSession(basar)
            .With(session => session.Seller, seller)
            .Create();
        ProductEntity product = _fixture.BuildProduct()
            .With(product => product.Session, session)
            .With(product => product.ValueState, valueState)
            .With(product => product.StorageState, storageState)
            .Create();
        session.Products.Add(product);
        Db.AcceptSessions.Add(session);
    }
}
