using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public class SettingsContext : ISettingsContext
    {
        private const string VeloSettingsContentType = "application/VeloSettings";
        private const string PrintSettingsContentType = "application/PrintSettings";
        private const int VeloSettingsId = 1;
        private const int PrintSettingsId = 2;

        private readonly VeloRepository _db;
        private readonly IFileStoreContext _fileContext;

        public SettingsContext(VeloRepository db, IFileStoreContext fileContext)
        {
            _db = db;
            _fileContext = fileContext;
        }

        public async Task<PrintSettings> GetPrintSettingsAsync() => await GetSettingsAsync<PrintSettings>(PrintSettingsId);
        public VeloSettings GetSettings() => GetSettings<VeloSettings>(VeloSettingsId);
        public async Task<VeloSettings> GetSettingsAsync() => await GetSettingsAsync<VeloSettings>(VeloSettingsId);
        public async Task UpdateAsync(PrintSettings toUpdate) => await UpdateAsync(toUpdate, PrintSettingsId, PrintSettingsContentType);
        public async Task UpdateAsync(VeloSettings toUpdate) => await UpdateAsync(toUpdate, VeloSettingsId, VeloSettingsContentType);

        private async Task UpdateAsync<T>(T instance, int id, string contentType) where T : class
        {
            FileData settingsFileStore;
            if (await _fileContext.ExistsAsync(id))
            {
                settingsFileStore = await _fileContext.GetAsync(id);
                settingsFileStore.Data = instance.SerializeAsJson();
            }
            else
            {
                settingsFileStore = new FileData
                {
                    ContentType = contentType,
                    Data = instance.SerializeAsJson()
                };
                await _db.Files.AddAsync(settingsFileStore);
            }
            await _db.SaveChangesAsync();

            if (settingsFileStore.Id != id)
            {
                throw new InvalidOperationException($"{typeof(T).Name} did't got the right ID.");
            }
        }
        private T GetSettings<T>(int id) where T : class
        {
            var fileStore = _db.Files.AsNoTracking().First(f => f.Id == id);
            return JsonUtils.DeserializeFromJson<T>(fileStore.Data);
        }
        private async Task<T> GetSettingsAsync<T>(int id) where T : class
        {
            var fileStore = await _db.Files.AsNoTracking().FirstAsync(f => f.Id == id);
            return JsonUtils.DeserializeFromJson<T>(fileStore.Data);
        }
    }
}
