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
        private readonly IBrandContext _brandContext;
        private readonly IProductTypeContext _productTypeContext;
        private readonly VeloRepository _db;

        public ProductContext(VeloRepository db, IBrandContext brandContext, IProductTypeContext productTypeContext)
        {
            _db = db;
            _brandContext = brandContext;
            _productTypeContext = productTypeContext;
        }

        public async Task<bool> ExistsAsync(int id) => await _db.Products.ExistsAsync(id);
        public async Task<Product> GetAsync(int id) => await _db.Products.IncludeAll().FirstOrDefaultAsync(p => p.Id == id);
        public IQueryable<Product> GetMany(IList<int> ids) => _db.Products.Where(p => ids.Contains(p.Id)).IncludeAll().DefaultOrder();
        public IQueryable<Product> GetProductsForBasar(Basar basar) => GetProductsForBasar(basar, null, null, null);
        public IQueryable<Product> GetProductsForBasar(Basar basar, string searchString, StorageState? storageState, ValueState? valueState)
        {
            var iq = _db.Products.IncludeAll().Where(p => p.BasarId == basar.Id);
            
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
            return _db.Products.IncludeAll().Where(p => p.BasarId == basar.Id && p.SellerId == sellerId);
        }
        public async Task<IList<Product>> InsertProductsAsync(Basar basar, Seller seller, IList<Product> products)
        {
            Contract.Requires(products != null);

            var newProducts = new List<Product>();
            foreach (var product in products)
            {
                var newProduct = new Product
                {
                    Basar = basar,
                    Brand = product.Brand,
                    Color = product.Color,
                    Description = product.Description,
                    FrameNumber = product.FrameNumber,
                    Label = product.Label,
                    Price = product.Price,
                    Seller = seller,
                    StorageState = StorageState.Available,
                    TireSize = product.TireSize,
                    Type = product.Type,
                    ValueState = ValueState.NotSettled
                };
                newProducts.Add(newProduct);
            }

            _db.Products.AddRange(newProducts);
            await _db.SaveChangesAsync();

            return newProducts;
        }
        public async Task ReloadRelationsAsync(IList<Product> products)
        {
            Contract.Requires(products != null);

            foreach (var product in products)
            {
                product.Brand = await _brandContext.GetAsync(product.Brand.Id);
                product.BrandId = product.Brand.Id;
                product.Type = await _productTypeContext.GetAsync(product.Type.Id);
                product.TypeId = product.Type.Id;
            }
        }
        public async Task UpdateAsync(Product product)
        {
            _db.Attach(product).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        private Expression<Func<Product, bool>> ProductSearch(string searchString)
        {
            if (_db.IsPostgreSQL())
            {
                return p => EF.Functions.ILike(p.Brand.Name, $"%{searchString}%")
                || EF.Functions.ILike(p.Color, $"%{searchString}%")
                || EF.Functions.ILike(p.Description, $"%{searchString}%")
                || EF.Functions.ILike(p.FrameNumber, $"%{searchString}%")
                || EF.Functions.ILike(p.TireSize, $"%{searchString}%")
                || EF.Functions.ILike(p.Type.Name, $"%{searchString}%");
            }
            return p => EF.Functions.Like(p.Brand.Name, $"%{searchString}%")
                || EF.Functions.Like(p.Color, $"%{searchString}%")
                || EF.Functions.Like(p.Description, $"%{searchString}%")
                || EF.Functions.Like(p.FrameNumber, $"%{searchString}%")
                || EF.Functions.Like(p.TireSize, $"%{searchString}%")
                || EF.Functions.Like(p.Type.Name, $"%{searchString}%");
        }
    }
}
