namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AdminServiceTests;

public class CreateSampleLabelsAsync
    : TestBase
{
    private const int _expectedProductCount = 3;

    [Theory]
    [VeloAutoData]
    public async void ShouldCreateRandomProductsAndDocument(SellerEntity seller, BasarEntity basar, ProductEntity product, byte[] documentBytes)
    {
        //  Arrange
        A.CallTo(() => DataGeneratorService.NextCountry()).Returns(seller.Country);
        A.CallTo(() => DataGeneratorService.NextSeller(seller.Country)).Returns(seller);
        A.CallTo(() => TokenProvider.CreateToken(seller)).Returns(seller.Token);
        A.CallTo(() => DataGeneratorService.NextBasar()).Returns(basar);
        A.CallTo(() => DataGeneratorService.NextBrand()).Returns(product.Brand);
        A.CallTo(() => DataGeneratorService.NextProductType()).Returns(product.Type);
        A.CallTo(() => DataGeneratorService.NextProduct(product.Brand, product.Type, A<AcceptSessionEntity>.Ignored)).Returns(product);
        A.CallTo(() => DocumentService.CreateLabelsAsync(A<IEnumerable<ProductEntity>>.Ignored)).Returns(documentBytes);

        //  Act
        FileDataEntity result = await Sut.CreateSampleLabelsAsync();

        //  Assert
        using (new AssertionScope())
        {
            result.ContentType.Should().Be(FileDataEntity.PdfContentType);
            result.Data.Should().BeEquivalentTo(documentBytes);
            result.FileName.Should().NotBeNullOrEmpty();
        }
        A.CallTo(() => DataGeneratorService.NextCountry()).MustHaveHappenedOnceExactly();
        A.CallTo(() => DataGeneratorService.NextSeller(seller.Country)).MustHaveHappenedOnceExactly();
        A.CallTo(() => TokenProvider.CreateToken(seller)).MustHaveHappenedOnceExactly();
        A.CallTo(() => DataGeneratorService.NextBasar()).MustHaveHappenedOnceExactly();
        A.CallTo(() => DataGeneratorService.NextBrand()).MustHaveHappened(_expectedProductCount, Times.Exactly);
        A.CallTo(() => DataGeneratorService.NextProductType()).MustHaveHappened(_expectedProductCount, Times.Exactly);
        A.CallTo(() => DataGeneratorService.NextProduct(product.Brand, product.Type, A<AcceptSessionEntity>.Ignored)).MustHaveHappened(_expectedProductCount, Times.Exactly);
        A.CallTo(() => DocumentService.CreateLabelsAsync(A<IEnumerable<ProductEntity>>.Ignored)).MustHaveHappenedOnceExactly();
    }
}
