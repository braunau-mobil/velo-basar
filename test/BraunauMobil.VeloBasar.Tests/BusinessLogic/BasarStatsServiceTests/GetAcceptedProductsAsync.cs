using Microsoft.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarStatsServiceTests;
public class GetAcceptedProductsAsync
    : TestBase<EmptySqliteDbFixture>
{
    private readonly Fixture _fixture = new();

    [Theory]
    [AutoData]
    public async Task NoProductsAtAll_ShouldReturnEmpty(int basarId)
    {
        //  Arrange

        //  Act
        IReadOnlyCollection<ProductEntity> products = await Sut.GetAcceptedProductsAsync(basarId);

        //  Assert
        products.Should().BeEmpty();
    }

    [Theory]
    [AutoData]
    public async Task BasarHasAcceptedProducts_ShouldReturnProducts(AcceptSessionEntity session1, AcceptSessionEntity session2)
    {
        //  Arrange
        AddProduct(StorageState.Available, session1);
        AddProduct(StorageState.Locked, session1);
        AddProduct(StorageState.Lost, session1);
        AddProduct(StorageState.NotAccepted, session1);
        AddProduct(StorageState.Sold, session1);
        AddProduct(StorageState.Available, session2);
        AddProduct(StorageState.Locked, session2);
        AddProduct(StorageState.Lost, session2);
        AddProduct(StorageState.NotAccepted, session2);
        AddProduct(StorageState.Sold, session2);
        await Db.SaveChangesAsync();

        //  Act
        var x = await Db.Products.AsNoTracking()
            .Include(product => product.Session)
            .Include(product => product.Type)
            .AsNoTracking().ToArrayAsync();
        IReadOnlyCollection<ProductEntity> products = await Sut.GetAcceptedProductsAsync(session1.BasarId);

        //  Assert
        using (new AssertionScope())
        {
            products.Should().HaveCount(4);
            products.Should().AllSatisfy(product => product.Session.BasarId.Should().Be(session1.BasarId));
            products.Should().AllSatisfy(product => product.StorageState.Should().NotBe(StorageState.NotAccepted));
        }
    }

    private void AddProduct(StorageState storageState, AcceptSessionEntity session)
    {
        ProductEntity product = _fixture.Build<ProductEntity>()
            .With(_ => _.StorageState, storageState)
            .With(_ => _.Session, session)
            .Create();

        Db.Products.Add(product);
    }
}
