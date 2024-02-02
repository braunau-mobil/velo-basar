using BraunauMobil.VeloBasar.Models.Documents;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.DocumentServiceTests;

public class CreateTransactionDocumentAsync
     : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task Acceptance_ShouldCallFactoryAndPassModelToGenerator(TransactionEntity transaction, AcceptanceDocumentModel documentModel, byte[] data)
    {
        //  Arrange
        transaction.Type = TransactionType.Acceptance;
        A.CallTo(() => Factory.CreateAcceptanceModel(transaction)).Returns(documentModel);
        A.CallTo(() => TransactionDocumentGenerator.CreateAcceptanceAsync(documentModel)).Returns(data);

        //  Act
        byte[] result = await Sut.CreateTransactionDocumentAsync(transaction);

        //  Assert
        result.Should().BeSameAs(data);
        A.CallTo(() => Factory.CreateAcceptanceModel(transaction)).MustHaveHappenedOnceExactly();
        A.CallTo(() => TransactionDocumentGenerator.CreateAcceptanceAsync(documentModel)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task Sale_ShouldCallFactoryAndPassModelToGenerator(TransactionEntity transaction, SaleDocumentModel documentModel, byte[] data)
    {
        //  Arrange
        transaction.Type = TransactionType.Sale;
        A.CallTo(() => Factory.CreateSaleModel(transaction)).Returns(documentModel);
        A.CallTo(() => TransactionDocumentGenerator.CreateSaleAsync(documentModel)).Returns(data);

        //  Act
        byte[] result = await Sut.CreateTransactionDocumentAsync(transaction);

        //  Assert
        result.Should().BeSameAs(data);
        A.CallTo(() => Factory.CreateSaleModel(transaction)).MustHaveHappenedOnceExactly();
        A.CallTo(() => TransactionDocumentGenerator.CreateSaleAsync(documentModel)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task Settlement_ShouldCallFactoryAndPassModelToGenerator(TransactionEntity transaction, SettlementDocumentModel documentModel, byte[] data)
    {
        //  Arrange
        transaction.Type = TransactionType.Settlement;
        A.CallTo(() => Factory.CreateSettlementModel(transaction)).Returns(documentModel);
        A.CallTo(() => TransactionDocumentGenerator.CreateSettlementAsync(documentModel)).Returns(data);

        //  Act
        byte[] result = await Sut.CreateTransactionDocumentAsync(transaction);

        //  Assert
        result.Should().BeSameAs(data);
        A.CallTo(() => Factory.CreateSettlementModel(transaction)).MustHaveHappenedOnceExactly();
        A.CallTo(() => TransactionDocumentGenerator.CreateSettlementAsync(documentModel)).MustHaveHappenedOnceExactly();
    }


    [Theory]
    [VeloInlineAutoData(TransactionType.Cancellation)]
    [VeloInlineAutoData(TransactionType.Lock)]
    [VeloInlineAutoData(TransactionType.SetLost)]
    [VeloInlineAutoData(TransactionType.Unlock)]
    public async Task AllOther_ShouldThrowInvalidOperationException(TransactionType transactionType, TransactionEntity transaction)
    {
        //  Arrange
        transaction.Type = transactionType;

        //  Act
        Func<Task> act = async () => await Sut.CreateTransactionDocumentAsync(transaction);

        //  Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage($"Cannot generate transaction document for: {transactionType}");
    }
}
