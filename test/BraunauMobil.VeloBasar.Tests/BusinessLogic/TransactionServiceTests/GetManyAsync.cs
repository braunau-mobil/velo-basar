namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.TransactionServiceTests;

public class GetManyAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task EmptyDatabase_ReturnsEmptyList(int pageSize, int pageIndex, int basarId, TransactionType transactionType, string searchString)
    {
        //  Arrange

        //  Act
        IReadOnlyCollection<TransactionEntity> transactions = await Sut.GetManyAsync(pageSize, pageIndex, basarId, transactionType, searchString);

        //  Assert
        transactions.Should().BeEmpty();
    }

    [Theory]
    [VeloAutoData]
    public async Task TransactionsExist_ReturnsAll(BasarEntity basar, TransactionEntity[] transactions)
    {
        //  Arrange
        Db.Basars.Add(basar);
        foreach (TransactionEntity transaction in transactions)
        {
            transaction.Basar = basar;
            transaction.BasarId = 0;
        }
        Db.Transactions.AddRange(transactions);
        await Db.SaveChangesAsync();

        //  Act
        IReadOnlyCollection<TransactionEntity> result = await Sut.GetManyAsync(int.MaxValue, 0, basar.Id, null, string.Empty);

        //  Assert
        result.Should().BeEquivalentTo(transactions);
    }

    [Theory]
    [VeloAutoData]
    public async Task TransactionsExist_ReturnsAllWithSameTransactionType(BasarEntity basar, TransactionEntity[] transactions, TransactionType transactionType)
    {
        //  Arrange
        await InsertTransactionsAsync(basar, transactions, transaction =>
        {
            transaction.Type = transactionType;
        });

        //  Act
        IReadOnlyCollection<TransactionEntity> result = await Sut.GetManyAsync(int.MaxValue, 0, basar.Id, transactionType, string.Empty);

        //  Assert
        result.Should().BeEquivalentTo(transactions);
    }

    [Theory]
    [VeloAutoData]
    public async Task TransactionsExist_ReturnsAllWithMatchInSellerFirstName(BasarEntity basar, TransactionEntity[] toFind, SellerEntity seller)
    {
        //  Arrange
        await InsertTransactionsAsync(basar, toFind, transaction =>
        {
            transaction.Seller = seller;
        });

        string searchString = seller.FirstName[1..^1];

        //  Act
        IReadOnlyCollection<TransactionEntity> result = await Sut.GetManyAsync(int.MaxValue, 0, basar.Id, null, searchString);

        //  Assert
        result.Should().BeEquivalentTo(toFind);
    }

    [Theory]
    [VeloAutoData]
    public async Task TransactionsExist_ReturnsAllWithMatchInSellerLastName(BasarEntity basar, TransactionEntity[] toFind, SellerEntity seller)
    {
        //  Arrange
        await InsertTransactionsAsync(basar, toFind, transaction =>
        {
            transaction.Seller = seller;
        });

        string searchString = seller.LastName[1..^1];

        //  Act
        IReadOnlyCollection<TransactionEntity> result = await Sut.GetManyAsync(int.MaxValue, 0, basar.Id, null, searchString);

        //  Assert
        result.Should().BeEquivalentTo(toFind);
    }

    [Theory]
    [VeloAutoData]
    public async Task TransactionsExist_ReturnsAllWithMatchInSellerCity(BasarEntity basar, TransactionEntity[] toFind, SellerEntity seller)
    {
        //  Arrange
        await InsertTransactionsAsync(basar, toFind, transaction =>
        {
            transaction.Seller = seller;
        });

        string searchString = seller.City[1..^1];

        //  Act
        IReadOnlyCollection<TransactionEntity> result = await Sut.GetManyAsync(int.MaxValue, 0, basar.Id, null, searchString);

        //  Assert
        result.Should().BeEquivalentTo(toFind);
    }

    [Theory]
    [VeloAutoData]
    public async Task TransactionsExist_ReturnsAllWithMatchInSellerCountryName(BasarEntity basar, TransactionEntity[] toFind, SellerEntity seller)
    {
        //  Arrange
        await InsertTransactionsAsync(basar, toFind, transaction =>
        {
            transaction.Seller = seller;
        });

        string searchString = seller.Country.Name[1..^1];

        //  Act
        IReadOnlyCollection<TransactionEntity> result = await Sut.GetManyAsync(int.MaxValue, 0, basar.Id, null, searchString);

        //  Assert
        result.Should().BeEquivalentTo(toFind);
    }

    [Theory]
    [VeloAutoData]
    public async Task TransactionsExist_ReturnsAllWithMatchInSellerBankAccountHolder(BasarEntity basar, TransactionEntity[] toFind, SellerEntity seller, string bankAccountHolder)
    {
        //  Arrange
        seller.BankAccountHolder = bankAccountHolder;
        await InsertTransactionsAsync(basar, toFind, transaction =>
        {
            transaction.Seller = seller;
        });

        string searchString = bankAccountHolder[1..^1];

        //  Act
        IReadOnlyCollection<TransactionEntity> result = await Sut.GetManyAsync(int.MaxValue, 0, basar.Id, null, searchString);

        //  Assert
        result.Should().BeEquivalentTo(toFind);
    }

    private async Task InsertTransactionsAsync(BasarEntity basar, TransactionEntity[] transactions, Action<TransactionEntity> adjustTransaction)
    {
        //  Arrange
        Db.Basars.Add(basar);
        foreach (TransactionEntity transaction in transactions)
        {
            adjustTransaction(transaction);
            transaction.Basar = basar;
            transaction.BasarId = 0;
        }
        Db.Transactions.AddRange(transactions);
        await Db.SaveChangesAsync();
    }
}
