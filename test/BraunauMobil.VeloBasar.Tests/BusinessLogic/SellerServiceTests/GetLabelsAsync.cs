using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.SellerServiceTests;

public class GetLabelsAsync
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [AutoData]
    public async Task HasNoProducts_ShouldReturnFileDataEntity(BasarEntity basar, SellerEntity seller, byte[] data)
    {
        //  Arrange
        Fixture fixture = new();
        Db.Basars.Add(basar);
        Db.Sellers.Add(seller);
        await Db.SaveChangesAsync();
        
        ProductLabelService.Setup(_ => _.CreateLabelsAsync(It.IsAny<IEnumerable<ProductEntity>>()))
            .ReturnsAsync(data);

        //  Act
        FileDataEntity result = await Sut.GetLabelsAsync(basar.Id, seller.Id);

        //  Assert
        result.Should().NotBeNull();
        result.FileName.Should().Be($"Seller-{seller.Id}_ProductLabels.pdf");
        result.Data.Should().BeSameAs(data);
        result.ContentType.Should().Be("application/pdf");
    }

    [Theory]
    [AutoData]
    public async Task HasProducts_ShouldReturnFileDataEntity(BasarEntity basar, SellerEntity seller, byte[] data)
    {
        //  Arrange
        Fixture fixture = new();
        IEnumerable<AcceptSessionEntity> sessions = fixture.Build<AcceptSessionEntity>()
            .With(_ => _.Basar, basar)
            .With(_ => _.BasarId, basar.Id)
            .With(_ => _.Seller, seller)
            .With(_ => _.SellerId, seller.Id)
            .Do(session =>
            {
                IEnumerable<ProductEntity> products = fixture.Build<ProductEntity>()
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

        ProductLabelService.Setup(_ => _.CreateLabelsAsync(It.IsAny<IEnumerable<ProductEntity>>()))
            .ReturnsAsync(data);

        //  Act
        FileDataEntity result = await Sut.GetLabelsAsync(basar.Id, seller.Id);

        //  Assert
        result.Should().NotBeNull();
        result.FileName.Should().Be($"Seller-{seller.Id}_ProductLabels.pdf");
        result.Data.Should().BeSameAs(data);
        result.ContentType.Should().Be("application/pdf");
    }
}
