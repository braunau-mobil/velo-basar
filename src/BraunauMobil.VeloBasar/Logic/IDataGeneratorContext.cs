using BraunauMobil.VeloBasar.Models;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface IDataGeneratorContext
    {
        Task GenerateAsync(DataGeneratorConfiguration config);
    }
}
