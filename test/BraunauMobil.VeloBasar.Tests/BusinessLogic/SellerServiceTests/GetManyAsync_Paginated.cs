using AutoFixture.Dsl;
using Xan.AspNetCore.Models;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SellerServiceTests;

public class GetManyAsync_Paginated
    : TestBase<EmptySqliteDbFixture>
{
    private readonly Fixture _fixture = new();

    [Theory]
    [AutoData]
    public async Task NoSellers_ShouldReturnEmpty(int pageSize, int pageIndex, string searchString, ObjectState objectState, ValueState valueState)
    {
        //  Arrange

        //  Act
        IPaginatedList<SellerEntity> result = await Sut.GetManyAsync(pageSize, pageIndex, searchString, objectState, valueState);

        //  Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [AutoData]
    public async Task NoSellerMatches_ShouldReturnEmpty(SellerEntity[] sellers, int pageSize, int pageIndex, string searchString, ObjectState objectState, ValueState valueState)
    {
        //  Arrange
        Db.Sellers.AddRange(sellers);
        await Db.SaveChangesAsync();

        //  Act
        IPaginatedList<SellerEntity> result = await Sut.GetManyAsync(pageSize, pageIndex, searchString, objectState, valueState);

        //  Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task PaginationWorks()
    {
        //  Arrange
        //int id = 1;
        SellerEntity[] sellers = BuildSeller()
            .CreateMany(100)
            .OrderBy(seller => seller.FirstName).ThenBy(seller => seller.LastName)
            .ToArray();
        Db.Sellers.AddRange(sellers);
        await Db.SaveChangesAsync();

        //  Act
        IPaginatedList<SellerEntity> result = await Sut.GetManyAsync(10, 2);

        //  Assert
        result.Should().HaveCount(10);
        result.Should().BeEquivalentTo(sellers.Skip(10).Take(10));
    }

    [Fact]
    public async Task SearchStringShouldMatchFields()
    {
        //  Arrange
        IEnumerable<SellerEntity> otherSellers = BuildSeller().CreateMany();
        Db.Sellers.AddRange(otherSellers);
        CountryEntity country = _fixture.Build<CountryEntity>().With(_ => _.Name, "uvwxyz").Create();
        SellerEntity[] sellersToFind = new[]
        {
            BuildSeller().With(_ => _.FirstName, "uvwxyz").Create(),
            BuildSeller().With(_ => _.LastName, "uvwxyz").Create(),
            BuildSeller().With(_ => _.Street, "uvwxyz").Create(),
            BuildSeller().With(_ => _.City, "uvwxyz").Create(),
            BuildSeller().With(_ => _.Country, country).Create(),
            BuildSeller().With(_ => _.BankAccountHolder, "uvwxyz").Create(),
        };
        Db.Sellers.AddRange(sellersToFind);
        await Db.SaveChangesAsync();

        //  Act
        IPaginatedList<SellerEntity> result = await Sut.GetManyAsync(10, 1, searchString: "wxy");

        //  Assert
        result.Should().BeEquivalentTo(sellersToFind);
    }

    [Theory]
    [AutoData]
    public async Task ObjectStateShouldMatch(ObjectState objectState)
    {
        //  Arrange        
        CountryEntity country = _fixture.Build<CountryEntity>().With(_ => _.Name, "uvwxyz").Create();
        SellerEntity[] sellersToFind = BuildSeller()
            .With(_ => _.State, objectState)
            .CreateMany().ToArray();
        Db.Sellers.AddRange(sellersToFind);
        _fixture.ExcludeEnumValues(objectState);
        IEnumerable<SellerEntity> otherSellers = BuildSeller().CreateMany();
        Db.Sellers.AddRange(otherSellers);
        await Db.SaveChangesAsync();

        //  Act
        IPaginatedList<SellerEntity> result = await Sut.GetManyAsync(10, 1, objectState: objectState);

        //  Assert
        result.Should().BeEquivalentTo(sellersToFind);
    }

    [Theory]
    [AutoData]
    public async Task ValueStateShouldMatch(ValueState valueState)
    {
        //  Arrange
        CountryEntity country = _fixture.Build<CountryEntity>().With(_ => _.Name, "uvwxyz").Create();
        SellerEntity[] sellersToFind = BuildSeller()
            .With(_ => _.ValueState, valueState)
            .CreateMany().ToArray();
        Db.Sellers.AddRange(sellersToFind);
        _fixture.ExcludeEnumValues(valueState);
        IEnumerable<SellerEntity> otherSellers = BuildSeller().CreateMany();
        Db.Sellers.AddRange(otherSellers);
        await Db.SaveChangesAsync();

        //  Act
        IPaginatedList<SellerEntity> result = await Sut.GetManyAsync(10, 1, valueState: valueState);

        //  Assert
        result.Should().BeEquivalentTo(sellersToFind);
    }

    private IPostprocessComposer<SellerEntity> BuildSeller()
    {
        return _fixture.Build<SellerEntity>()
            .Without(_ => _.Id);
    }
}
