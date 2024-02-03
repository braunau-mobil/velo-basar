using BraunauMobil.VeloBasar.Data;
using Microsoft.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests.Data.ProductExtensionsTests;

public class GetForSellerAsync
    : DbTestBase<EmptySqliteDbFixture>
{
    private readonly VeloFixture _fixture = new();

    [Theory]
    [VeloAutoData]
    public async Task ShouldReturnOnlyProductsForSpecifiedBasarAndSeller(BasarEntity basarToLookFor, SellerEntity sellerToLookFor, SellerEntity[] otherSellers)
    {
        // Arrange
        InsertProductsForSeller(sellerToLookFor, basarToLookFor);
        foreach (SellerEntity seller in otherSellers)
        {
            InsertProductsForSeller(seller);
        }
        await Db.SaveChangesAsync();

        // Act
        IReadOnlyList<ProductEntity> result = await Db.Products.WhereBasarAndSeller(basarToLookFor.Id, sellerToLookFor.Id).ToArrayAsync();

        // Assert
        result.Should().AllSatisfy(product =>
        {
            product.Session.BasarId.Should().Be(basarToLookFor.Id);
            product.Session.SellerId.Should().Be(sellerToLookFor.Id);
        });
    }

    private void InsertProductsForSeller(SellerEntity seller)
        => InsertProductsForSeller(seller, _fixture.Create<BasarEntity>());

    private void InsertProductsForSeller(SellerEntity seller, BasarEntity basarToLookFor)
    {
        AcceptSessionEntity session = _fixture.BuildAcceptSession()
            .With(session => session.Seller, seller)
            .With(session => session.Basar, basarToLookFor)
            .Create();

        IEnumerable<ProductEntity> procucts = _fixture.BuildProduct()
            .With(product => product.Session, session)
            .CreateMany();

        Db.Products.AddRange(procucts);
    }
}
