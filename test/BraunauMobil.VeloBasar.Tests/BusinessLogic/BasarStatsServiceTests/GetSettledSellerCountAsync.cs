namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;

public class GetSettledSellerCountAsync
    : TestBase<EmptySqliteDbFixture>
{
    private readonly Fixture _fixture = new();

    [Theory]
    [AutoData]
    public async Task NoSellersAtAll_ShouldReturnZero(int basarId)
    {
        //  Arrange

        //  Act
        int count = await Sut.GetSettledSellerCountAsync(basarId);

        //  Assert
        count.Should().Be(0);

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task BasarHasNoSellers_ShouldReturnZero(BasarEntity basar, BasarEntity otherBasar)
    {
        //  Arrange
        Db.Basars.Add(basar);
        IEnumerable<TransactionEntity> otherTransactions = _fixture.BuildTransaction()
            .With(_ => _.Seller)
            .WithBasar(otherBasar)
            .CreateMany();
        Db.Transactions.AddRange(otherTransactions);
        await Db.SaveChangesAsync();

        //  Act
        int count = await Sut.GetSettledSellerCountAsync(basar.Id);

        //  Assert
        count.Should().Be(0);

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task BasarHasSellers_ShouldReturnCount(BasarEntity basar, BasarEntity otherBasar)
    {
        //  Arrange
        CountryEntity country = _fixture.Create<CountryEntity>();
        IEnumerable<SellerEntity> settledSellers = _fixture.Build<SellerEntity>()
            .With(_ => _.Country, country)
            .With(_ => _.CountryId, country.Id)
            .With(_ => _.ValueState, ValueState.Settled)
            .CreateMany();
        foreach (SellerEntity seller in settledSellers)
        {
            IEnumerable<TransactionEntity> acceptances = _fixture
                .BuildTransaction()
                .WithBasar(basar)
                .With(_ => _.Type, TransactionType.Acceptance)
                .With(_ => _.Seller, seller)
                .With(_ => _.SellerId, seller.Id)
                .CreateMany();
            Db.Transactions.AddRange(acceptances);            
            Db.Sellers.Add(seller);
        }

        IEnumerable<SellerEntity> unSettledSellers = _fixture.Build<SellerEntity>()
            .With(_ => _.Country, country)
            .With(_ => _.CountryId, country.Id)
            .With(_ => _.ValueState, ValueState.NotSettled)
            .CreateMany();
        foreach (SellerEntity seller in unSettledSellers)
        {
            IEnumerable<TransactionEntity> acceptances = _fixture
                .BuildTransaction()
                .WithBasar(basar)
                .With(_ => _.Type, TransactionType.Acceptance)
                .With(_ => _.Seller, seller)
                .With(_ => _.SellerId, seller.Id)
                .CreateMany();
            Db.Transactions.AddRange(acceptances);
            Db.Sellers.Add(seller);
        }

        IEnumerable<TransactionEntity> otherTransactions = _fixture.BuildTransaction()
            .WithBasar(otherBasar)
            .Without(_ => _.Seller)
            .Without(_ => _.SellerId)
            .CreateMany();
        Db.Transactions.AddRange(otherTransactions);
        await Db.SaveChangesAsync();

        //  Act
        int count = await Sut.GetSettledSellerCountAsync(basar.Id);

        //  Assert
        count.Should().Be(settledSellers.Count());

        VerifyNoOtherCalls();
    }
}
