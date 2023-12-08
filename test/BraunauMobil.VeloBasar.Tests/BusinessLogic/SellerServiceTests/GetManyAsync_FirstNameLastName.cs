namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SellerServiceTests;

public class GetManyAsync_FirstNameLastName
    : TestBase<EmptySqliteDbFixture>
{
    private readonly Fixture _fixture = new();
    
    [Theory]
    [AutoData]
    public async Task NoSellers_ShouldReturnEmpty(string firstName, string lastName)
    {
        //  Arrange

        //  Act
        IReadOnlyList<SellerEntity> result = await Sut.GetManyAsync(firstName, lastName);

        //  Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [AutoData]
    public async Task NoSellerMatches_ShouldReturnEmpty(SellerEntity[] sellers, string firstName, string lastName)
    {
        //  Arrange
        Db.Sellers.AddRange(sellers);
        await Db.SaveChangesAsync();

        //  Act
        IReadOnlyList<SellerEntity> result = await Sut.GetManyAsync(firstName, lastName);

        //  Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task MatchFirstName_ShouldReturnMatchedSllers()
    {
        //  Arrange
        IEnumerable<SellerEntity> otherSellers = _fixture.BuildSeller().CreateMany();
        Db.Sellers.AddRange(otherSellers);

        SellerEntity aragog = _fixture.BuildSeller()
            .With(_ => _.FirstName, "Aragog")
            .Create();
        SellerEntity aragorn = _fixture.BuildSeller()
            .With(_ => _.LastName, "Aragorn")
            .Create();
        SellerEntity aragogAragorn = _fixture.BuildSeller()
            .With(_ => _.FirstName, "Aragog")
            .With(_ => _.LastName, "Aragorn")
            .Create();
        Db.Sellers.AddRange(aragorn, aragog, aragogAragorn);
        await Db.SaveChangesAsync();

        //  Act
        IReadOnlyList<SellerEntity> result = await Sut.GetManyAsync("Arag", "");

        //  Assert
        result.Should().HaveCount(2);
        result.Any(_ => _.Id == aragog.Id).Should().BeTrue();
        result.Any(_ => _.Id == aragogAragorn.Id).Should().BeTrue();
    }

    [Fact]
    public async Task MatchLastName_ShouldReturnMatchedSllers()
    {
        //  Arrange
        IEnumerable<SellerEntity> otherSellers = _fixture.BuildSeller().CreateMany();
        Db.Sellers.AddRange(otherSellers);

        SellerEntity aragog = _fixture.BuildSeller()
            .With(_ => _.FirstName, "Aragog")
            .Create();
        SellerEntity aragorn = _fixture.BuildSeller()
            .With(_ => _.LastName, "Aragorn")
            .Create();
        SellerEntity aragogAragorn = _fixture.BuildSeller()
            .With(_ => _.FirstName, "Aragog")
            .With(_ => _.LastName, "Aragorn")
            .Create();
        Db.Sellers.AddRange(aragorn, aragog, aragogAragorn);
        await Db.SaveChangesAsync();

        //  Act
        IReadOnlyList<SellerEntity> result = await Sut.GetManyAsync("", "Arag");

        //  Assert
        result.Should().HaveCount(2);
        result.Any(_ => _.Id == aragorn.Id).Should().BeTrue();
        result.Any(_ => _.Id == aragogAragorn.Id).Should().BeTrue();
    }

    [Fact]
    public async Task MatchBoth_ShouldReturnMatchedSllers()
    {
        //  Arrange
        IEnumerable<SellerEntity> otherSellers = _fixture.BuildSeller().CreateMany();
        Db.Sellers.AddRange(otherSellers);

        SellerEntity aragog = _fixture.BuildSeller()
            .With(_ => _.FirstName, "Aragog")
            .Create();
        SellerEntity aragorn = _fixture.BuildSeller()
            .With(_ => _.LastName, "Aragorn")
            .Create();
        SellerEntity aragogAragorn = _fixture.BuildSeller()
            .With(_ => _.FirstName, "Aragog")
            .With(_ => _.LastName, "Aragorn")
            .Create();
        Db.Sellers.AddRange(aragorn, aragog, aragogAragorn);
        await Db.SaveChangesAsync();

        //  Act
        IReadOnlyList<SellerEntity> result = await Sut.GetManyAsync("Arag", "Arag");

        //  Assert
        result.Should().HaveCount(1);
        result.Any(_ => _.Id == aragogAragorn.Id).Should().BeTrue();
    }
}
