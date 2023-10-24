using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Models.Entities;

namespace BraunauMobil.VeloBasar.DataGenerator.Mockups;

public class StatusPushServiceMock
    : IStatusPushService
{
    public Task PushAwayAsync(TransactionEntity transaction)
    {
        return Task.CompletedTask;
    }
}
