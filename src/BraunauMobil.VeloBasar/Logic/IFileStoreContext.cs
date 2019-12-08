using BraunauMobil.VeloBasar.Models;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface IFileStoreContext
    {
        Task<bool> ExistsAsync(int id);
        Task<FileData> GetAsync(int id);
    }
}
