using BraunauMobil.VeloBasar.Models;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface INumberContext
    {
        int NextNumber(Basar basar, TransactionType transactionType);
        Task CreateNewNumberAsync(Basar basar, TransactionType type);
    }
}
