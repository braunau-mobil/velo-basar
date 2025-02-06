using BraunauMobil.VeloBasar.Configuration;
using BraunauMobil.VeloBasar.Models.Documents;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.DocumentServiceTests;

public class CreateLabelAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ShouldCallFactoryAndPassModelToGenerator(ProductEntity product, ProductLabelDocumentModel productLabelDocumentModel, byte[] data, LabelPrintSettings labelPrintSettings)
    {
        //  Arrange
        A.CallTo(() => Factory.CreateProductLabelModel(product)).Returns(productLabelDocumentModel);
        A.CallTo(() => Factory.LabelPrintSettings).Returns(labelPrintSettings);
        A.CallTo(() => ProductLabelGenerator.CreateLabelAsync(productLabelDocumentModel, labelPrintSettings)).Returns(data);

        //  Act
        byte[] result = await Sut.CreateLabelAsync(product);

        //  Assert
        result.Should().BeSameAs(data);
        A.CallTo(() => Factory.CreateProductLabelModel(product)).MustHaveHappenedOnceExactly();
        A.CallTo(() => Factory.LabelPrintSettings).MustHaveHappenedOnceExactly();
        A.CallTo(() => ProductLabelGenerator.CreateLabelAsync(productLabelDocumentModel, labelPrintSettings)).MustHaveHappenedOnceExactly();
    }
}
