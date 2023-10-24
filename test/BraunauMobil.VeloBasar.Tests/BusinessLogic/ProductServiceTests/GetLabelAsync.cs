namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.ProductServiceTests;

public class GetLabelAsync
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [AutoData]
    public void EmptyDatabase_Throws(int productId)
    {
        //  Arrange

        //  Act
        Func<Task> act = async () => await Sut.GetLabelAsync(productId);

        //  Assert
        act.Should().ThrowAsync<InvalidOperationException>();

        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async void ProductExists_ReturnsLabelDataAsPdf(ProductEntity product, byte[] labelData)
    {
        //  Arrange
        product.Id = 1;
        Db.Products.Add(product);
        await Db.SaveChangesAsync();
        ProductLabelService.Setup(_ => _.CreateLabelAsync(product))
            .ReturnsAsync(labelData);

        //  Act
        FileDataEntity fileData = await Sut.GetLabelAsync(product.Id);
        
        //  Assert
        fileData.ContentType.Should().Be(FileDataEntity.PdfContentType);
        fileData.Data.Should().BeEquivalentTo(labelData);
        fileData.FileName.Should().Be("Product-1_Label.pdf");
        ProductLabelService.Verify(_ => _.CreateLabelAsync(product), Times.Once());
        VerifyNoOtherCalls();
    }
}
