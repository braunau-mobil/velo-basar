using BraunauMobil.VeloBasar.Models.Documents;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.DocumentServiceTests;

public class CreateLabelAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ShouldCallFactoryAndPassModelToGenerator(ProductEntity product, ProductLabelDocumentModel productLabelDocumentModel, byte[] data)
    {
        //  Arrange
        A.CallTo(() => Factory.CreateProductLabelModel(product)).Returns(productLabelDocumentModel);
        A.CallTo(() => ProductLabelGenerator.CreateLabelAsync(productLabelDocumentModel)).Returns(data);

        //  Act
        byte[] result = await Sut.CreateLabelAsync(product);

        //  Assert
        result.Should().BeSameAs(data);
        A.CallTo(() => Factory.CreateProductLabelModel(product)).MustHaveHappenedOnceExactly();
        A.CallTo(() => ProductLabelGenerator.CreateLabelAsync(productLabelDocumentModel)).MustHaveHappenedOnceExactly();
    }
}
