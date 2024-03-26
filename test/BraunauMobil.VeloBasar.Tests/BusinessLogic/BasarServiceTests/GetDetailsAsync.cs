namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarServiceTests;

public sealed class GetDetailsAsync
    : TestBase
{    
    [Theory]
    [VeloAutoData]
    public async Task ValuesShouldBeCorrect(BasarDetailsModel details, ProductEntity[] acceptedProducts, Tuple<TimeOnly, decimal>[] transactionsAndTotals)
    {
        //  Arrange
        Db.Basars.Add(details.Entity);
        await Db.SaveChangesAsync();
        int basarId = details.Entity.Id;
        
        A.CallTo(() => StatsService.GetAcceptanceCountAsync(basarId)).Returns(details.AcceptanceCount);
        A.CallTo(() => StatsService.GetSaleCountAsync(basarId)).Returns(details.SaleCount);
        A.CallTo(() => StatsService.GetSettlementStatusAsync(basarId)).Returns(details.SettlementStatus);
        A.CallTo(() => StatsService.GetAcceptedProductsAsync(basarId)).Returns(acceptedProducts);
        A.CallTo(() => StatsService.GetSoldProductTimestampsAndPricesAsync(basarId)).Returns(transactionsAndTotals);
        A.CallTo(() => StatsService.GetAcceptedProductsAmount(acceptedProducts)).Returns(details.AcceptedProductsAmount);
        A.CallTo(() => StatsService.GetAcceptedProductsCount(acceptedProducts)).Returns(details.AcceptedProductsCount);
        A.CallTo(() => StatsService.GetAcceptedProductTypesWithAmount(acceptedProducts)).Returns(details.AcceptedProductTypesByAmount);
        A.CallTo(() => StatsService.GetAcceptedProductTypesWithCount(acceptedProducts)).Returns(details.AcceptedProductTypesByCount);
        A.CallTo(() => StatsService.GetLostProductsCount(acceptedProducts)).Returns(details.LostProductsCount);
        A.CallTo(() => StatsService.GetLockedProductsCount(acceptedProducts)).Returns(details.LockedProductsCount);
        A.CallTo(() => StatsService.GetPriceDistribution(acceptedProducts)).Returns(details.PriceDistribution);
        A.CallTo(() => StatsService.GetSaleDistribution(transactionsAndTotals)).Returns(details.SaleDistribution);
        A.CallTo(() => StatsService.GetSellerCountAsync(basarId)).Returns(details.SellerCount);
        A.CallTo(() => StatsService.GetSoldProductsAmount(acceptedProducts)).Returns(details.SoldProductsAmount);
        A.CallTo(() => StatsService.GetSoldProductsCount(acceptedProducts)).Returns(details.SoldProductsCount);
        A.CallTo(() => StatsService.GetSoldProductTypesWithAmount(acceptedProducts)).Returns(details.SoldProductTypesByAmount);
        A.CallTo(() => StatsService.GetSoldProductTypesWithCount(acceptedProducts)).Returns(details.SoldProductTypesByCount);

        //  Act
        BasarDetailsModel result = await Sut.GetDetailsAsync(basarId);

        //  Assert
        result.Should().BeEquivalentTo(details);

        // add verifications for all mocks
        result.Entity.Should().BeEquivalentTo(details.Entity);
        A.CallTo(() => StatsService.GetAcceptanceCountAsync(basarId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatsService.GetSaleCountAsync(basarId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatsService.GetSettlementStatusAsync(basarId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatsService.GetAcceptedProductsAsync(basarId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatsService.GetSoldProductTimestampsAndPricesAsync(basarId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatsService.GetAcceptedProductsAmount(acceptedProducts)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatsService.GetAcceptedProductsCount(acceptedProducts)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatsService.GetAcceptedProductTypesWithAmount(acceptedProducts)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatsService.GetAcceptedProductTypesWithCount(acceptedProducts)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatsService.GetLostProductsCount(acceptedProducts)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatsService.GetLockedProductsCount(acceptedProducts)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatsService.GetPriceDistribution(acceptedProducts)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatsService.GetSaleDistribution(transactionsAndTotals)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatsService.GetSellerCountAsync(basarId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatsService.GetSoldProductsAmount(acceptedProducts)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatsService.GetSoldProductsCount(acceptedProducts)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatsService.GetSoldProductTypesWithAmount(acceptedProducts)).MustHaveHappenedOnceExactly();
        A.CallTo(() => StatsService.GetSoldProductTypesWithCount(acceptedProducts)).MustHaveHappenedOnceExactly();
    }
}
