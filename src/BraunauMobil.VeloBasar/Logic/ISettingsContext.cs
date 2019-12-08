using BraunauMobil.VeloBasar.Models;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface ISettingsContext
    {
        VeloSettings GetSettings();
        Task<VeloSettings> GetSettingsAsync();
        Task<PrintSettings> GetPrintSettingsAsync();
        Task UpdateAsync(PrintSettings toUpdate);
        Task UpdateAsync(VeloSettings toUpdate);
    }
}
