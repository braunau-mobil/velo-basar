namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class GetLabelAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public void EmptyDatabase_Throws(int productId)
    {
        //  Arrange

        //  Act
        Func<Task> act = async () => await Sut.GetLabelAsync(productId);

        //  Assert
        act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Theory]
    [VeloAutoData]
    public async Task ProductExists_ReturnsLabelDataAsPdf(ProductEntity product, byte[] labelData)
    {
        //  Arrange
        product.Id = 1;
        Db.Products.Add(product);
        await Db.SaveChangesAsync();
        A.CallTo(() => DocumentService.CreateLabelAsync(product)).Returns(labelData);

        //  Act
        FileDataEntity fileData = await Sut.GetLabelAsync(product.Id);

        //  Assert
        using (new AssertionScope())
        {
            fileData.ContentType.Should().Be(FileDataEntity.PdfContentType);
            fileData.Data.Should().BeEquivalentTo(labelData);
            fileData.FileName.Should().Be("VeloBasar_LabelFileName_1");
        }
        A.CallTo(() => DocumentService.CreateLabelAsync(product)).MustHaveHappenedOnceExactly();
    }
}
