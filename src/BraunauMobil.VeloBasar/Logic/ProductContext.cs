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
        private readonly IBasarContext _basarContext;
        private readonly IBrandContext _brandContext;
        private readonly IProductTypeContext _productTypeContext;
        private readonly IFileStoreContext _fileStoreContext;
        private readonly ISettingsContext _settingsContext;
        private readonly VeloRepository _db;

        public ProductContext(VeloRepository db, IBasarContext basarContext, IBrandContext brandContext, IProductTypeContext productTypeContext, IFileStoreContext fileStoreContext, ISettingsContext settingsContext)
        {
            _db = db;
            _basarContext = basarContext;
            _brandContext = brandContext;
            _productTypeContext = productTypeContext;
            _fileStoreContext = fileStoreContext;
            _settingsContext = settingsContext;
        }

        public async Task<bool> ExistsAsync(int id) => await _db.Products.ExistsAsync(id);
        public async Task<Product> GetAsync(int id) => await _db.Products.IncludeAll().FirstOrDefaultAsync(p => p.Id == id);
        public IQueryable<Product> GetMany(IList<int> ids) => _db.Products.Where(p => ids.Contains(p.Id)).IncludeAll().DefaultOrder();
        public IQueryable<Product> GetProductsForBasar(Basar basar) => GetProductsForBasar(basar, null, null, null, null, null);
        public IQueryable<Product> GetProductsForBasar(Basar basar, string searchString, StorageState? storageState, ValueState? valueState, int? brandId, int? productTypeId)
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

            if (brandId != null)
            {
                iq = iq.Where(p => p.BrandId == brandId.Value);
            }

            if (productTypeId != null)
            {
                iq = iq.Where(p => p.TypeId == productTypeId.Value);
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

            var printSettings = await _settingsContext.GetPrintSettingsAsync();

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
                    Price = product.Price,
                    Seller = seller,
                    StorageState = StorageState.Available,
                    TireSize = product.TireSize,
                    Type = product.Type,
                    ValueState = ValueState.NotSettled
                };
                newProduct.LabelId = await _fileStoreContext.CreateProductLabelAsync(newProduct, printSettings);
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
                await ReloadRelationsAsync(product);
            }
        }
        public async Task UpdateAsync(Product product)
        {
            Contract.Requires(product != null);

            await LoadMissingRelationsAsync(product);

            var printSettings = await _settingsContext.GetPrintSettingsAsync();
            if (product.LabelId != 0)
            {
                await _fileStoreContext.UpdateProductLabelAsync(product, printSettings);
            }
            else
            {
                product.LabelId = await _fileStoreContext.CreateProductLabelAsync(product, printSettings);
            }

            _db.Attach(product).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        private async Task LoadMissingRelationsAsync(Product product)
        {
            if (product.Brand == null)
            {
                product.Brand = await _brandContext.GetAsync(product.BrandId);
                product.BrandId = product.Brand.Id;
            }
            if (product.Basar == null)
            {
                product.Basar = await _basarContext.GetAsync(product.BasarId);
                product.BasarId = product.Basar.Id;
            }
            if (product.Type == null)
            {
                product.Type = await _productTypeContext.GetAsync(product.TypeId);
                product.TypeId = product.Type.Id;
            }
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
        private async Task ReloadRelationsAsync(Product product)
        {
            product.Brand = await _brandContext.GetAsync(product.Brand.Id);
            product.BrandId = product.Brand.Id;
            product.Basar = await _basarContext.GetAsync(product.Basar.Id);
            product.BasarId = product.Basar.Id;
            product.Type = await _productTypeContext.GetAsync(product.Type.Id);
            product.TypeId = product.Type.Id;
        }
    }
}
