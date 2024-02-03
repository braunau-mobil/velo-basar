namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.TransactionServiceTests;

public class GetDocumentAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task HasDocumenAndUpdateOnDemandIsFalse_ShouldFetchDocumentFromDb(TransactionEntity transaction, FileDataEntity document)
    {
        // Arrange
        Db.Files.Add(document);
        await Db.SaveChangesAsync();
        transaction.Type = TransactionType.Sale;
        transaction.DocumentId = document.Id;
        Db.Transactions.Add(transaction);
        await Db.SaveChangesAsync();

        //  Act
        FileDataEntity result = await Sut.GetDocumentAsync(transaction.Id);

        //  Assert
        result.Should().BeEquivalentTo(document);
    }

    [Theory]
    [VeloAutoData]
    public async Task HasDocumenAndUpdateOnDemandIsTrue_ShouldRegenerateDocument(TransactionEntity transaction, FileDataEntity document, byte[] data)
    {
        // Arrange
        Db.Files.Add(document);
        await Db.SaveChangesAsync();
        transaction.TimeStamp = X.FirstContactDay;
        transaction.Type = TransactionType.Acceptance;
        transaction.DocumentId = document.Id;
        Db.Transactions.Add(transaction);
        await Db.SaveChangesAsync();

        A.CallTo(() => DocumentService.CreateTransactionDocumentAsync(transaction)).Returns(data);

        //  Act
        FileDataEntity result = await Sut.GetDocumentAsync(transaction.Id);

        //  Assert
        using (new AssertionScope())
        {
            result.ContentType.Should().Be(FileDataEntity.PdfContentType);
            result.Data.Should().BeEquivalentTo(data);
            result.FileName.Should().BeEquivalentTo("2063-04-05T11:22:33_VeloBasar_AcceptanceSingular-1.pdf");
        }
        A.CallTo(() => DocumentService.CreateTransactionDocumentAsync(transaction)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task HasNoDocument_ShouldRegenerateDocument(TransactionEntity transaction, byte[] data)
    {
        // Arrange
        transaction.Type = TransactionType.Acceptance;
        transaction.TimeStamp = X.FirstContactDay;
        Db.Transactions.Add(transaction);
        await Db.SaveChangesAsync();

        A.CallTo(() => DocumentService.CreateTransactionDocumentAsync(transaction)).Returns(data);

        //  Act
        FileDataEntity result = await Sut.GetDocumentAsync(transaction.Id);

        //  Assert
        using (new AssertionScope())
        {
            result.ContentType.Should().Be(FileDataEntity.PdfContentType);
            result.Data.Should().BeEquivalentTo(data);
            result.FileName.Should().BeEquivalentTo("2063-04-05T11:22:33_VeloBasar_AcceptanceSingular-1.pdf");
        }
        A.CallTo(() => DocumentService.CreateTransactionDocumentAsync(transaction)).MustHaveHappenedOnceExactly();
    }
}
