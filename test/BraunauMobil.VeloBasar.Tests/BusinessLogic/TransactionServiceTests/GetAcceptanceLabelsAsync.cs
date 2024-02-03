namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.TransactionServiceTests;

public class GetAcceptanceLabelsAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ShouldCreateLabelsForAllProductsOfOneTransaction(TransactionEntity acceptance, ProductEntity[] products, byte[] data)
    {
        //  Arrange
        acceptance.TimeStamp = X.FirstContactDay;
        acceptance.Type = TransactionType.Acceptance;
        foreach (ProductEntity product in products)
        {
            acceptance.Products.Add(new ProductToTransactionEntity {  Transaction = acceptance, Product = product });
        }
        Db.Transactions.Add(acceptance);
        await Db.SaveChangesAsync();

        A.CallTo(() => DocumentService.CreateLabelsAsync(products)).Returns(data);

        //  Act
        FileDataEntity result = await Sut.GetAcceptanceLabelsAsync(acceptance.Id);

        //  Assert
        using (new AssertionScope())
        {
            result.ContentType.Should().Be(FileDataEntity.PdfContentType);
            result.Data.Should().BeEquivalentTo(data);
            result.FileName.Should().Be("2063-04-05T11:22:33_VeloBasar_AcceptanceSingular-1_VeloBasar_Labels.pdf");
        }
        A.CallTo(() => DocumentService.CreateLabelsAsync(products)).MustHaveHappenedOnceExactly();
    }
}
