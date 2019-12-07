using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Data
{
    public interface INumberPool
    {
        int NextNumber(Basar basar, TransactionType transactionType);
    }
}
