using BraunauMobil.VeloBasar.Parameters;
using Xan.AspNetCore.Models;
using Xan.AspNetCore.Mvc.Crud;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SellerServiceTests;

public class GetManyAsync_Paginated
    : TestBase<EmptySqliteDbFixture>
{
    private readonly VeloFixture _fixture = new();

    [Theory]
    [VeloAutoData]
    public async Task NoSellers_ShouldReturnEmpty(SellerListParameter parameter)
    {
        //  Arrange

        //  Act
        IPaginatedList<CrudItemModel<SellerEntity>> result = await Sut.GetManyAsync(parameter);

        //  Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [VeloAutoData]
    public async Task NoSellerMatches_ShouldReturnEmpty(SellerEntity[] sellers, SellerListParameter parameter)
    {
        //  Arrange
        Db.Sellers.AddRange(sellers);
        await Db.SaveChangesAsync();

        //  Act
        IPaginatedList<CrudItemModel<SellerEntity>> result = await Sut.GetManyAsync(parameter);

        //  Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task PaginationWorks()
    {
        //  Arrange
        SellerListParameter parameter = new()
        {
            PageSize = 10,
            PageIndex = 2
        };
        SellerEntity[] sellers = _fixture.CreateMany<SellerEntity>(100)
            .OrderBy(seller => seller.FirstName).ThenBy(seller => seller.LastName)
            .ToArray();
        Db.Sellers.AddRange(sellers);
        await Db.SaveChangesAsync();

        //  Act
        IPaginatedList<CrudItemModel<SellerEntity>> result = await Sut.GetManyAsync(parameter);

        //  Assert
        result.Should().HaveCount(10);
        result.Select(model => model.Entity).Should().BeEquivalentTo(sellers.Skip(10).Take(10));
    }

    [Fact]
    public async Task SearchStringShouldMatchFields()
    {
        //  Arrange
        SellerListParameter parameter = new()
        {
            PageSize = 10,
            PageIndex = 1,
            SearchString = "wxy"
        };
        IEnumerable<SellerEntity> otherSellers = _fixture.CreateMany<SellerEntity>();
        Db.Sellers.AddRange(otherSellers);
        CountryEntity country = _fixture.BuildCountry().With(_ => _.Name, "uvwxyz").Create();
        SellerEntity[] sellersToFind = new[]
        {
            _fixture.BuildSeller().With(_ => _.FirstName, "uvwxyz").Create(),
            _fixture.BuildSeller().With(_ => _.LastName, "uvwxyz").Create(),
            _fixture.BuildSeller().With(_ => _.Street, "uvwxyz").Create(),
            _fixture.BuildSeller().With(_ => _.City, "uvwxyz").Create(),
            _fixture.BuildSeller().With(_ => _.Country, country).Create(),
            _fixture.BuildSeller().With(_ => _.BankAccountHolder, "uvwxyz").Create(),
        };
        Db.Sellers.AddRange(sellersToFind);
        await Db.SaveChangesAsync();

        //  Act
        IPaginatedList<CrudItemModel<SellerEntity>> result = await Sut.GetManyAsync(parameter);

        //  Assert
        result.Select(model => model.Entity).Should().BeEquivalentTo(sellersToFind);
    }

    [Theory]
    [VeloAutoData]
    public async Task ObjectStateShouldMatch(ObjectState objectState)
    {
        //  Arrange
        SellerListParameter parameter = new()
        {
            PageSize = 10,
            PageIndex = 1,
            State = objectState
        };
        CountryEntity country = _fixture.BuildCountry().With(_ => _.Name, "uvwxyz").Create();
        SellerEntity[] sellersToFind = _fixture.BuildSeller()
            .With(_ => _.State, objectState)
            .CreateMany().ToArray();
        Db.Sellers.AddRange(sellersToFind);
        _fixture.ExcludeEnumValues(objectState);
        IEnumerable<SellerEntity> otherSellers = _fixture.CreateMany<SellerEntity>();
        Db.Sellers.AddRange(otherSellers);
        await Db.SaveChangesAsync();

        //  Act
        IPaginatedList<CrudItemModel<SellerEntity>> result = await Sut.GetManyAsync(parameter);

        //  Assert
        result.Select(model => model.Entity).Should().BeEquivalentTo(sellersToFind);
    }

    [Theory]
    [VeloAutoData]
    public async Task ValueStateShouldMatch(ValueState valueState)
    {
        //  Arrange
        SellerListParameter parameter = new()
        {
            PageSize = 10,
            PageIndex = 1,
            ValueState = valueState
        };
        CountryEntity country = _fixture.BuildCountry().With(_ => _.Name, "uvwxyz").Create();
        SellerEntity[] sellersToFind = _fixture.BuildSeller()
            .With(_ => _.ValueState, valueState)
            .CreateMany().ToArray();
        Db.Sellers.AddRange(sellersToFind);
        _fixture.ExcludeEnumValues(valueState);
        IEnumerable<SellerEntity> otherSellers = _fixture.CreateMany<SellerEntity>();
        Db.Sellers.AddRange(otherSellers);
        await Db.SaveChangesAsync();

        //  Act
        IPaginatedList<CrudItemModel<SellerEntity>> result = await Sut.GetManyAsync(parameter);

        //  Assert
        result.Select(model => model.Entity).Should().BeEquivalentTo(sellersToFind);
    }
}
