namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SellerServiceTests;

public class GetLabelsAsync
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [VeloAutoData]
    public async Task HasNoProducts_ShouldReturnFileDataEntity(BasarEntity basar, SellerEntity seller, byte[] data)
    {
        //  Arrange
        VeloFixture fixture = new();
        Db.Basars.Add(basar);
        Db.Sellers.Add(seller);
        await Db.SaveChangesAsync();

        A.CallTo(() => ProductLabelService.CreateLabelsAsync(A<IEnumerable<ProductEntity>>._)).Returns(data);

        //  Act
        FileDataEntity result = await Sut.GetLabelsAsync(basar.Id, seller.Id);

        //  Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.FileName.Should().Be($"Seller-{seller.Id}_ProductLabels.pdf");
            result.Data.Should().BeSameAs(data);
            result.ContentType.Should().Be("application/pdf");
        }
    }

    [Theory]
    [VeloAutoData]
    public async Task HasProducts_ShouldReturnFileDataEntity(BasarEntity basar, SellerEntity seller, byte[] data)
    {
        //  Arrange
        VeloFixture fixture = new();
        IEnumerable<AcceptSessionEntity> sessions = fixture.BuildAcceptSession(basar)
            .With(_ => _.Seller, seller)
            .With(_ => _.SellerId, seller.Id)
            .Do(session =>
            {
                IEnumerable<ProductEntity> products = fixture.BuildProduct()
                    .With(_ => _.Session, session)
                    .With(_ => _.SessionId, session.Id)
                    .CreateMany();
                foreach (ProductEntity product in products)
                {
                    session.Products.Add(product);
                }
            })
            .CreateMany();
        Db.AcceptSessions.AddRange(sessions);
        await Db.SaveChangesAsync();

        A.CallTo(() => ProductLabelService.CreateLabelsAsync(A<IEnumerable<ProductEntity>>._)).Returns(data);

        //  Act
        FileDataEntity result = await Sut.GetLabelsAsync(basar.Id, seller.Id);

        //  Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.FileName.Should().Be($"Seller-{seller.Id}_ProductLabels.pdf");
            result.Data.Should().BeSameAs(data);
            result.ContentType.Should().Be("application/pdf");
        }
    }
}
