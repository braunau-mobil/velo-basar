namespace BraunauMobil.VeloBasar.Tests.Rendering.SelectListServiceTests;

public class BrandsAsync
    : TestBase
{
    private readonly Fixture _fixture = new();

    [Theory]
    [AutoData]
    public async Task ShouldReturnAllBrandsFromProducts(AcceptSessionEntity session, string[] brands)
    {
        //  Arrange
        foreach (string brand in brands)
        {
            Db.Products.AddRange(_fixture.BuildProductEntity()
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
