using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetSellerCountAsync
    : TestBase
{
    private readonly VeloFixture _fixture = new();

    [Theory]
    [VeloAutoData]
    public async Task NoSellersAtAllShouldReturnZero(BasarEntity basar)
    {
        //  Arrange
        Db.Basars.Add(basar);
        await Db.SaveChangesAsync();

        //  Act
        int result = await Sut.GetSellerCountAsync(basar.Id);

        //  Assert
        result.Should().Be(0);
    }

    [Theory]
    [VeloAutoData]
    public async Task SellersButAllForOldBasar_ShouldReturnZero(BasarEntity oldBasar, BasarEntity newBasar)
    {
        //  Arrange
        TransactionEntity[] oldAcceptances = _fixture.BuildTransaction(oldBasar)
            .With(transaction => transaction.Type, TransactionType.Acceptance)
            .CreateMany().ToArray();
        Db.Transactions.AddRange(oldAcceptances);
        Db.Basars.Add(newBasar);
        await Db.SaveChangesAsync();

        //  Act
        int result = await Sut.GetSellerCountAsync(newBasar.Id);

        //  Assert
        result.Should().Be(0);
    }

    [Theory]
    [VeloAutoData]
    public async Task SellersWithAcceptances_ShouldReturnSellerCount(BasarEntity oldBasar, BasarEntity newBasar)
    {
        //  Arrange
        TransactionEntity[] oldAcceptances = _fixture.BuildTransaction(oldBasar)
            .With(transaction => transaction.Type, TransactionType.Acceptance)
            .CreateMany().ToArray();
        Db.Transactions.AddRange(oldAcceptances);
        TransactionEntity[] newAcceptances = _fixture.BuildTransaction(newBasar)
            .With(transaction => transaction.Type, TransactionType.Acceptance)
            .CreateMany().ToArray();
        Db.Transactions.AddRange(newAcceptances);
        await Db.SaveChangesAsync();

        //  Act
        int result = await Sut.GetSellerCountAsync(newBasar.Id);

        //  Assert
        result.Should().Be(newAcceptances.Length);
    }
}
