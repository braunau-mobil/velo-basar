namespace BraunauMobil.VeloBasar.Tests.Rendering.SelectListServiceTests;

public class BrandsAsync
    : TestBase
{
    private readonly VeloFixture _fixture = new();

    [Theory]
    [VeloAutoData]
    public async Task ShouldReturnAllBrandsFromProducts(AcceptSessionEntity session, string[] brands)
    {
        //  Arrange
        foreach (string brand in brands)
        {
            Db.Products.AddRange(_fixture.BuildProduct()
                .With(p => p.Brand, brand)
                .With(p => p.Session, session)
                .CreateMany());
        }
        await Db.SaveChangesAsync();

        //  Act
        ISet<string?> result = await Sut.BrandsAsync();

        //  Assert
        result.Should().BeEquivalentTo(brands);
    }
}
