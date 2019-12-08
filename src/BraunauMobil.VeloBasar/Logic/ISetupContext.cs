using BraunauMobil.VeloBasar.Models;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface ISetupContext
    {
        Task InitializeDatabaseAsync(InitializationConfiguration config);
    }
}
