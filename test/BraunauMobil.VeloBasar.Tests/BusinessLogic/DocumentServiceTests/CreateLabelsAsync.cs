using BraunauMobil.VeloBasar.Configuration;
using BraunauMobil.VeloBasar.Models.Documents;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.DocumentServiceTests;

public class CreateLabelsAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ShouldCallFactoryAndPassModelToGenerator(ProductEntity[] products, byte[] data, LabelPrintSettings labelPrintSettings)
    {
        //  Arrange
        A.CallTo(() => Factory.LabelPrintSettings).Returns(labelPrintSettings);
        A.CallTo(() => ProductLabelGenerator.CreateLabelsAsync(A<IEnumerable<ProductLabelDocumentModel>>.Ignored, labelPrintSettings)).Returns(data);

        //  Act
        byte[] result = await Sut.CreateLabelsAsync(products);

        //  Assert
        result.Should().BeSameAs(data);
        A.CallTo(() => Factory.LabelPrintSettings).MustHaveHappenedOnceExactly();
        A.CallTo(() => ProductLabelGenerator.CreateLabelsAsync(A<IEnumerable<ProductLabelDocumentModel>>.Ignored, labelPrintSettings)).MustHaveHappenedOnceExactly();
    }
}
