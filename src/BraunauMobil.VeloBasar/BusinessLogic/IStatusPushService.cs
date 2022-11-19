namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface IStatusPushService
{
    Task PushAwayAsync(TransactionEntity transaction);
}
