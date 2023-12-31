using Microsoft.AspNetCore.Mvc.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Rendering.SelectListServiceTests;

public class BrandsForSelectionAsync
    : TestBase
{
    private readonly Fixture _fixture = new();

    [Theory]
    [AutoData]
    public async Task Default_ShouldReturnAllBrandsFromProducts(AcceptSessionEntity session, string brand1, string brand2)
    {
        //  Arrange
        Db.Products.AddRange(_fixture.BuildProductEntity()
            .With(p => p.Brand, brand2)
            .With(p => p.Session, session)
            .CreateMany());
        Db.Products.AddRange(_fixture.BuildProductEntity()
            .With(p => p.Brand, brand1)
            .With(p => p.Session, session)
            .CreateMany());
        await Db.SaveChangesAsync();

        //  Act
        SelectList result = await Sut.BrandsForSelectionAsync();

        //  Assert
        result.Should().SatisfyRespectively(
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be(brand1);
                item.Value.Should().Be(brand1);
            },
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be(brand2);
                item.Value.Should().Be(brand2);
            }
        );
    }

    [Theory]
    [AutoData]
    public async Task IncludeAll_ShouldReturnAllBrandsFromProductsIncludingAllItem(AcceptSessionEntity session, string brand1, string brand2)
    {
        //  Arrange
        Db.Products.AddRange(_fixture.BuildProductEntity()
            .With(p => p.Brand, brand2)
            .With(p => p.Session, session)
            .CreateMany());
        Db.Products.AddRange(_fixture.BuildProductEntity()
            .With(p => p.Brand, brand1)
            .With(p => p.Session, session)
            .CreateMany());
        await Db.SaveChangesAsync();

        //  Act
        SelectList result = await Sut.BrandsForSelectionAsync(includeAll: true);

        //  Assert
        result.Should().SatisfyRespectively(
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be("VeloBasar_AllBrand");
                item.Value.Should().Be("");
            },
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be(brand1);
                item.Value.Should().Be(brand1);
            },
            item =>
            {
                item.Disabled.Should().BeFalse();
                item.Group.Should().BeNull();
                item.Selected.Should().BeFalse();
                item.Text.Should().Be(brand2);
                item.Value.Should().Be(brand2);
            }
        );
    }
}
