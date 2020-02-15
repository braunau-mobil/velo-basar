using BraunauMobil.VeloBasar.Models;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface ISetupContext
    {
        Task CreateDatabaseAsync();
        Task InitializeDatabaseAsync(InitializationConfiguration config);
    }
}
