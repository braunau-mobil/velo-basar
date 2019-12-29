using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public class ProductContext : IProductContext
    {
        private readonly VeloRepository _db;

        public ProductContext(VeloRepository db)
        {
            _db = db;
        }

        public void AttachRelations(IList<Product> products)
        {
            Contract.Requires(products != null);

            foreach (var product in products)
            {
                AttachRelations(product);
            }
        }
        public async Task<bool> ExistsAsync(int id) => await _db.ProductTypes.ExistsAsync(id);
        public async Task<Product> GetAsync(int id) => await _db.Products.IncludeAll().FirstOrDefaultAsync(p => p.Id == id);
        public IQueryable<Product> GetMany(IList<int> ids) => _db.Products.Where(p => ids.Contains(p.Id)).IncludeAll().DefaultOrder();
        public IQueryable<Product> GetProductsForBasar(Basar basar) => GetProductsForBasar(basar, null, null, null);
        public IQueryable<Product> GetProductsForBasar(Basar basar, string searchString, StorageState? storageState, ValueState? valueState)
        {
            var iq = _db.Transactions.IncludeAll().GetMany(TransactionType.Acceptance, basar).SelectMany(a => a.Products).Select(pa => pa.Product);
            
            if (!string.IsNullOrEmpty(searchString))
            {
                iq = iq.Where(ProductSearch(searchString));
            }

            if (storageState != null)
            {
                iq = iq.Where(p => p.StorageState == storageState.Value);
            }

            if (valueState != null)
            {
                iq = iq.Where(p => p.ValueState == valueState.Value);
            }

            return iq.DefaultOrder();
        }
        public IQueryable<Product> GetProductsForSeller(Basar basar, int sellerId)
        {
            return _db.Transactions.IncludeAll().GetMany(TransactionType.Acceptance, basar, sellerId).SelectMany(a => a.Products).Select(pa => pa.Product);
        }
        public async Task UpdateAsync(Product product)
        {
            _db.Attach(product).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        private void AttachRelations(Product product)
        {
            _db.Attach(product.Brand);
            _db.Attach(product.Type);
        }

        private static Expression<Func<Product, bool>> ProductSearch(string searchString)
        {
            return p => EF.Functions.Like(p.Brand.Name, $"%{searchString}%")
                || EF.Functions.Like(p.Color, $"%{searchString}%")
                || EF.Functions.Like(p.Description, $"%{searchString}%")
                || EF.Functions.Like(p.FrameNumber, $"%{searchString}%")
                || EF.Functions.Like(p.TireSize, $"%{searchString}%")
                || EF.Functions.Like(p.Type.Name, $"%{searchString}%");
        }
    }
}
