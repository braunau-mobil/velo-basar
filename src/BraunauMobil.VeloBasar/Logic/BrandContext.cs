using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public class BrandContext : IBrandContext
    {
        private readonly VeloRepository _db;
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;

        public BrandContext(VeloRepository dbContext, IStringLocalizer<SharedResource> stringLocalizer)
        {
            _db = dbContext;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<bool> CanDeleteAsync(Brand item) => !await _db.Products.AnyAsync(p => p.BrandId == item.Id);
        public async Task<Brand> CreateAsync(Brand brand)
        {
            _db.Brands.Add(brand);
            await _db.SaveChangesAsync();
            return brand;
        }
        public async Task DeleteAsync(int id)
        {
            var brand = await GetAsync(id);
            if (brand != null)
            {
                _db.Brands.Remove(brand);
                await _db.SaveChangesAsync();
            }
        }
        public async Task<bool> ExistsAsync(int id) => await _db.Brands.ExistsAsync(id);
        public async Task<Brand> GetAsync(int id) => await _db.Brands.FirstOrDefaultAsync(p => p.Id == id);
        public IQueryable<Brand> GetMany(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return _db.Brands.DefaultOrder();
            }
            return _db.Brands.Where(BrandSearch(searchString)).DefaultOrder();
        }

        public SelectList GetSelectList() => new SelectList(_db.Brands.DefaultOrder(), "Id", "Name");
        public SelectList GetSelectListWithAllItem()
        {
            var brands = new List<Tuple<int?, string>>
            {
                new Tuple<int?, string>(null, _stringLocalizer["Alle"])
            };
            brands.AddRange(_db.Brands.DefaultOrder().Select(b => new Tuple<int?, string>(b.Id, b.Name)));
            return new SelectList(brands, "Item1", "Item2");
        }
        public async Task UpdateAsync(Brand brand)
        {
            _db.Attach(brand).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        private Expression<Func<Brand, bool>> BrandSearch(string searchString)
        {
            if (int.TryParse(searchString, out int id))
            {
                return b => b.Id == id;
            }
            if (_db.IsPostgreSQL())
            {
                return b => EF.Functions.ILike(b.Name, $"%{searchString}%");
            }
            return b => EF.Functions.Like(b.Name, $"%{searchString}%");
        }
    }
}
