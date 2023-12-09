namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarServiceTests;

public sealed class GetDetailsAsync
    : TestBase
{    
    [Theory]
    [AutoData]
    public async Task ValuesShouldBeCorrect(BasarDetailsModel details, ProductEntity[] acceptedProducts, Tuple<TimeOnly, decimal>[] transactionsAndTotals)
    {
        //  Arrange
        Db.Basars.Add(details.Entity);
        await Db.SaveChangesAsync();
        int basarId = details.Entity.Id;
        
        StatsService.Setup(_ => _.GetAcceptanceCountAsync(basarId))
            .ReturnsAsync(details.AcceptanceCount);
        StatsService.Setup(_ => _.GetSaleCountAsync(basarId))
            .ReturnsAsync(details.SaleCount);
        StatsService.Setup(_ => _.GetSettlementStatusAsync(basarId))
            .ReturnsAsync(details.SettlementStatus);
        StatsService.Setup(_ => _.GetAcceptedProductsAsync(basarId))
            .ReturnsAsync(acceptedProducts);
        StatsService.Setup(_ => _.GetSoldProductTimestampsAndPricesAsync(basarId))
            .ReturnsAsync(transactionsAndTotals);
        StatsService.Setup(_ => _.GetAcceptedProductsAmount(acceptedProducts))
            .Returns(details.AcceptedProductsAmount);
        StatsService.Setup(_ => _.GetAcceptedProductsCount(acceptedProducts))
            .Returns(details.AcceptedProductsCount);
        StatsService.Setup(_ => _.GetAcceptedProductTypesWithAmount(acceptedProducts))
            .Returns(details.AcceptedProductTypesByAmount);
        StatsService.Setup(_ => _.GetAcceptedProductTypesWithCount(acceptedProducts))
            .Returns(details.AcceptedProductTypesByCount);
        StatsService.Setup(_ => _.GetLostProductsCount(acceptedProducts))
            .Returns(details.LostProductsCount);
        StatsService.Setup(_ => _.GetLockedProductsCount(acceptedProducts))
            .Returns(details.LockedProductsCount);
        StatsService.Setup(_ => _.GetPriceDistribution(acceptedProducts))
            .Returns(details.PriceDistribution);
        StatsService.Setup(_ => _.GetSaleDistribution(transactionsAndTotals))
            .Returns(details.SaleDistribution);
        StatsService.Setup(_ => _.GetSoldProductsAmount(acceptedProducts))
            .Returns(details.SoldProductsAmount);
        StatsService.Setup(_ => _.GetSoldProductsCount(acceptedProducts))
            .Returns(details.SoldProductsCount);
        StatsService.Setup(_ => _.GetSoldProductTypesWithAmount(acceptedProducts))
            .Returns(details.SoldProductTypesByAmount);
        StatsService.Setup(_ => _.GetSoldProductTypesWithCount(acceptedProducts))
            .Returns(details.SoldProductTypesByCount);

        //  Act
        BasarDetailsModel result = await Sut.GetDetailsAsync(basarId);

        //  Assert
        result.Should().BeEquivalentTo(details);

        // add verifications for all mocks
        result.Entity.Should().BeEquivalentTo(details.Entity);
        StatsService.Verify(_ => _.GetAcceptanceCountAsync(basarId), Times.Once);
        StatsService.Verify(_ => _.GetSaleCountAsync(basarId), Times.Once);
        StatsService.Verify(_ => _.GetSettlementStatusAsync(basarId), Times.Once);
        StatsService.Verify(_ => _.GetAcceptedProductsAsync(basarId), Times.Once);
        StatsService.Verify(_ => _.GetSoldProductTimestampsAndPricesAsync(basarId), Times.Once);
        StatsService.Verify(_ => _.GetAcceptedProductsAmount(acceptedProducts), Times.Once);
        StatsService.Verify(_ => _.GetAcceptedProductsCount(acceptedProducts), Times.Once);
        StatsService.Verify(_ => _.GetAcceptedProductTypesWithAmount(acceptedProducts), Times.Once);
        StatsService.Verify(_ => _.GetAcceptedProductTypesWithCount(acceptedProducts), Times.Once);
        StatsService.Verify(_ => _.GetLostProductsCount(acceptedProducts), Times.Once);
        StatsService.Verify(_ => _.GetLockedProductsCount(acceptedProducts), Times.Once);
        StatsService.Verify(_ => _.GetPriceDistribution(acceptedProducts), Times.Once);
        StatsService.Verify(_ => _.GetSaleDistribution(transactionsAndTotals), Times.Once);
        StatsService.Verify(_ => _.GetSoldProductsAmount(acceptedProducts), Times.Once);
        StatsService.Verify(_ => _.GetSoldProductsCount(acceptedProducts), Times.Once);
        StatsService.Verify(_ => _.GetSoldProductTypesWithAmount(acceptedProducts), Times.Once);
        StatsService.Verify(_ => _.GetSoldProductTypesWithCount(acceptedProducts), Times.Once);

        VerifyNoOtherCalls();
    }
}
