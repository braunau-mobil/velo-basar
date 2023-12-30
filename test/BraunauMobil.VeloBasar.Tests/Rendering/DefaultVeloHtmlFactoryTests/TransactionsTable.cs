using Xan.AspNetCore.Rendering;

namespace BraunauMobil.VeloBasar.Tests.Rendering.DefaultVeloHtmlFactoryTests;

public class TransactionsTable
    : TestBase
{
    [Fact]
    public void DefaultConfiguration()
    {
        //  Arrange
        IEnumerable<TransactionEntity> transactions = Enumerable.Empty<TransactionEntity>();

        //  Act
        TableBuilder<TransactionEntity> table = Sut.TransactionsTable(transactions);

        //  Assert
        table.Should().SatisfyRespectively(
            column => column.Title.Should().BeHtml("VeloBasar_Number"),
            column => column.Title.Should().BeHtml("VeloBasar_TimeStamp"),
            column => column.Title.Should().BeHtml("VeloBasar_Notes"),
            column => column.Title.Should().BeHtml(""),
            column => column.Title.Should().BeHtml("")
        );
    }

    [Fact]
    public void ShowType()
    {
        //  Arrange
        IEnumerable<TransactionEntity> transactions = Enumerable.Empty<TransactionEntity>();

        //  Act
        TableBuilder<TransactionEntity> table = Sut.TransactionsTable(transactions, showType: true);

        //  Assert
        table.Should().SatisfyRespectively(
            column => column.Title.Should().BeHtml("VeloBasar_Number"),
            column => column.Title.Should().BeHtml("VeloBasar_TimeStamp"),
            column => column.Title.Should().BeHtml("VeloBasar_Type"),
            column => column.Title.Should().BeHtml("VeloBasar_Notes"),
            column => column.Title.Should().BeHtml(""),
            column => column.Title.Should().BeHtml("")
        );
    }

    [Fact]
    public void ShowProducts()
    {
        //  Arrange
        IEnumerable<TransactionEntity> transactions = Enumerable.Empty<TransactionEntity>();

        //  Act
        TableBuilder<TransactionEntity> table = Sut.TransactionsTable(transactions, showProducts: true);

        //  Assert
        table.Should().SatisfyRespectively(
            column => column.Title.Should().BeHtml("VeloBasar_Number"),
            column => column.Title.Should().BeHtml("VeloBasar_TimeStamp"),
            column => column.Title.Should().BeHtml("VeloBasar_ProductCount"),
            column => column.Title.Should().BeHtml("VeloBasar_Notes"),
            column => column.Title.Should().BeHtml("VeloBasar_Sum"),
            column => column.Title.Should().BeHtml(""),
            column => column.Title.Should().BeHtml("")
        );
    }

    [Fact]
    public void ShowTypeAndProducts()
    {
        //  Arrange
        IEnumerable<TransactionEntity> transactions = Enumerable.Empty<TransactionEntity>();

        //  Act
        TableBuilder<TransactionEntity> table = Sut.TransactionsTable(transactions, showType: true, showProducts: true);

        //  Assert
        table.Should().SatisfyRespectively(
            column => column.Title.Should().BeHtml("VeloBasar_Number"),
            column => column.Title.Should().BeHtml("VeloBasar_TimeStamp"),
            column => column.Title.Should().BeHtml("VeloBasar_Type"),
            column => column.Title.Should().BeHtml("VeloBasar_ProductCount"),
            column => column.Title.Should().BeHtml("VeloBasar_Notes"),
            column => column.Title.Should().BeHtml("VeloBasar_Sum"),
            column => column.Title.Should().BeHtml(""),
            column => column.Title.Should().BeHtml("")
        );
    }
}
