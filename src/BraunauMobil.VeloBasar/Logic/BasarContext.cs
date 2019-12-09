using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public class BasarContext : IBasarContext
    {
        private readonly VeloRepository _db;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly INumberContext _numberPool;
        private readonly ISettingsContext _settingsContext;

        public BasarContext(VeloRepository context, IStringLocalizer<SharedResource> localizer, INumberContext numberPool, ISettingsContext settingsContext)
        {
            _db = context;
            _localizer = localizer;
            _numberPool = numberPool;
            _settingsContext = settingsContext;
        }

        public async Task<bool> CanDeleteAsync(Basar basar) => !await _db.Transactions.AnyAsync(t => t.BasarId == basar.Id);
        public async Task<Basar> CreateAsync(Basar toCreate)
        {
            Contract.Requires(toCreate != null);

            _db.Basars.Add(toCreate);

            foreach (var enumValue in Enum.GetValues(typeof(TransactionType)))
            {
                await _numberPool.CreateNewNumberAsync(toCreate, (TransactionType)enumValue);
            }
            await _db.SaveChangesAsync();

            if (await _db.Basars.CountAsync() == 1)
            {
                var settings = await _settingsContext.GetSettingsAsync();
                settings.ActiveBasarId = toCreate.Id;
                await _settingsContext.UpdateAsync(settings);
            }
            return toCreate;
        }
        public async Task DeleteAsync(int basarId)
        {
            var basar = await GetAsync(basarId);
            if (basar != null && !await CanDeleteAsync(basar))
            {
                throw new InvalidOperationException(_localizer["Basar mit ID={0} kann nicht gelöscht werden.", basar.Id]);
            }

            foreach (var enumValue in Enum.GetValues(typeof(TransactionType)))
            {
                var transactionType = (TransactionType)enumValue;
                var number = await _db.Numbers.FirstAsync(n => n.BasarId == basar.Id && n.Type == transactionType);
                _db.Numbers.Remove(number);
            }

            _db.Basars.Remove(basar);
            await _db.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int id) => await _db.Basars.ExistsAsync(id);
        public bool Exists(int id) => _db.Basars.Any(b => b.Id == id);
        public async Task<Basar> GetAsync(int id) => await _db.Basars.FirstOrDefaultAsync(p => p.Id == id);
        public Basar Get(int id) => _db.Basars.FirstOrDefault(p => p.Id == id);
        public IQueryable<Basar> GetMany(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return _db.Basars.DefaultOrder();
            }
            return _db.Basars.Where(BasarSearch(searchString)).DefaultOrder();
        }
        public SelectList GetSelectList() => new SelectList(_db.Basars, "Id", "Name");
        public bool HasBasars()
        {
            if (_db.IsInitialized())
            {
                return _db.Basars.Any();
            }
            return false;
        }
        public async Task UpdateAsync(Basar toUpdate)
        {
            _db.Attach(toUpdate).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        private static Expression<Func<Basar, bool>> BasarSearch(string searchString)
        {
            if (int.TryParse(searchString, out int id))
            {
                return b => b.Id == id;
            }
            return b => b.Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
