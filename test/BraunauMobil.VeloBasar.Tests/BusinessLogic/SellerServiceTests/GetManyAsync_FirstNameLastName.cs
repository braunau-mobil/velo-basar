using AutoFixture.Dsl;

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
        Fixture fixture = new();
        IEnumerable<SellerEntity> otherSellers = BuildSeller().CreateMany();
        Db.Sellers.AddRange(otherSellers);

        SellerEntity aragog = fixture.Build<SellerEntity>()
            .With(_ => _.FirstName, "Aragog")
            .Create();
        SellerEntity aragorn = fixture.Build<SellerEntity>()
            .With(_ => _.LastName, "Aragorn")
            .Create();
        SellerEntity aragogAragorn = fixture.Build<SellerEntity>()
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
        Fixture fixture = new();
        IEnumerable<SellerEntity> otherSellers = BuildSeller().CreateMany();
        Db.Sellers.AddRange(otherSellers);

        SellerEntity aragog = BuildSeller()
            .With(_ => _.FirstName, "Aragog")
            .Create();
        SellerEntity aragorn = BuildSeller()
            .With(_ => _.LastName, "Aragorn")
            .Create();
        SellerEntity aragogAragorn = BuildSeller()
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
        Fixture fixture = new();
        IEnumerable<SellerEntity> otherSellers = BuildSeller().CreateMany();
        Db.Sellers.AddRange(otherSellers);

        SellerEntity aragog = fixture.Build<SellerEntity>()
            .With(_ => _.FirstName, "Aragog")
            .Create();
        SellerEntity aragorn = fixture.Build<SellerEntity>()
            .With(_ => _.LastName, "Aragorn")
            .Create();
        SellerEntity aragogAragorn = fixture.Build<SellerEntity>()
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

    private IPostprocessComposer<SellerEntity> BuildSeller()
    {
        return _fixture.Build<SellerEntity>()
            .Without(_ => _.Id);
    }
}
