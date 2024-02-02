using BraunauMobil.VeloBasar.Models.Documents;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.DocumentServiceTests;

public class CreateLabelsAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ShouldCallFactoryAndPassModelToGenerator(ProductEntity[] products, ProductLabelDocumentModel productLabelDocumentModel, byte[] data)
    {
        //  Arrange
        A.CallTo(() => ProductLabelGenerator.CreateLabelsAsync(A<IEnumerable<ProductLabelDocumentModel>>.Ignored)).Returns(data);

        //  Act
        byte[] result = await Sut.CreateLabelsAsync(products);

        //  Assert
        result.Should().BeSameAs(data);
        A.CallTo(() => ProductLabelGenerator.CreateLabelsAsync(A<IEnumerable<ProductLabelDocumentModel>>.Ignored)).MustHaveHappenedOnceExactly();
    }
}
