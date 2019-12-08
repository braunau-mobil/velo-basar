using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public class ProductTypeContext : IProductTypeContext
    {
        private readonly VeloRepository _db;

        public ProductTypeContext(VeloRepository db)
        {
            _db = db;
        }

        public async Task<bool> CanDeleteAsync(ProductType type) => !await _db.Products.AnyAsync(p => p.TypeId == type.Id);
        public async Task<ProductType> CreateAsync(ProductType type)
        {
            await _db.ProductTypes.AddAsync(type);
            await _db.SaveChangesAsync();
            return type;
        }
        public async Task DeleteAsync(int id)
        {
            var productType = await GetAsync(id);
            if (productType != null)
            {
                _db.ProductTypes.Remove(productType);
                await _db.SaveChangesAsync();
            }
        }
        public async Task<bool> ExistsAsync(int id) => await _db.ProductTypes.ExistsAsync(id);
        public async Task<ProductType> GetAsync(int id) => await _db.ProductTypes.FirstOrDefaultAsync(p => p.Id == id);
        public IQueryable<ProductType> GetMany(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return _db.ProductTypes.DefaultOrder();
            }
            return _db.ProductTypes.Where(ProductTypeSearch(searchString)).DefaultOrder();
        }
        public SelectList GetSelectList() => new SelectList(_db.ProductTypes, "Id", "Name");
        public async Task SetStateAsync(int id, ObjectState state)
        {
            var productType = await GetAsync(id);
            productType.State = state;
            _db.Attach(productType).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
        public async Task UpdateAsync(ProductType toUpdate)
        {
            _db.Attach(toUpdate).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        private static Expression<Func<ProductType, bool>> ProductTypeSearch(string searchString)
        {
            if (int.TryParse(searchString, out int id))
            {
                return b => b.Id == id;
            }
            return b => b.Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
