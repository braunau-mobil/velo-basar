namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface INumberService
{
    Task<int> NextNumberAsync(int basarId, TransactionType transactionType);
}
