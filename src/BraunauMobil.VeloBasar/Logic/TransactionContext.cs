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
    public class TransactionContext : ITransactionContext
    {
        private readonly VeloRepository _db;
        private readonly INumberContext _numberContext;
        private readonly IProductContext _productContext;
        private readonly ISettingsContext _settingsContext;
        private readonly IFileStoreContext _fileContext;
        private readonly ISellerContext _sellerContext;
        private readonly IStatusPushService _productStatusService;

        public TransactionContext(VeloRepository db, INumberContext numberContext, IProductContext productContext, ISettingsContext settingsContext, IFileStoreContext fileContext, ISellerContext sellerContext, IStatusPushService salesStatusService)
        {
            _db = db;
            _numberContext = numberContext;
            _productContext = productContext;
            _settingsContext = settingsContext;
            _fileContext = fileContext;
            _sellerContext = sellerContext;
            _productStatusService = salesStatusService;
        }

        public async Task<ProductsTransaction> AcceptProductsAsync(Basar basar, int sellerId, IList<Product> products)
        {
            Contract.Requires(products != null);
            
            var seller = await _sellerContext.GetAsync(sellerId);
            var printSettings = await _settingsContext.GetPrintSettingsAsync();
            var productsInserted = await _productContext.InsertProductsAsync(basar, seller, products);

            var tx = await CreateTransactionAsync(basar, TransactionType.Acceptance, seller, printSettings, productsInserted.ToArray());

            await _sellerContext.SetValueStateAsync(sellerId, ValueState.NotSettled);
            return tx;
        }
        public async Task<ProductsTransaction> CancelProductsAsync(Basar basar, int saleId, IList<int> productIds)
        {
            var products = await _productContext.GetMany(productIds).ToArrayAsync();
            if (!products.IsAllowed(TransactionType.Cancellation))
            {
                return null;
            }

            products.SetState(TransactionType.Cancellation);
            await _db.SaveChangesAsync();

            var sale = await GetAsync(saleId);
            await RemoveProductsFromTransactionAsync(sale, products);

            var printSettings = await _settingsContext.GetPrintSettingsAsync();

            return await CreateTransactionAsync(basar, TransactionType.Cancellation, printSettings, products.ToArray());
        }
        public async Task<ProductsTransaction> CheckoutProductsAsync(Basar basar, IList<int> productIds)
        {
            var products = _productContext.GetMany(productIds);
            var printSettings = await _settingsContext.GetPrintSettingsAsync();
            return await DoTransactionAsync(basar, TransactionType.Sale, null, printSettings, await products.ToArrayAsync());
        }
        public async Task<ProductsTransaction> DoTransactionAsync(Basar basar, TransactionType transactionType, string notes, int productId)
        {
            var product = await _productContext.GetAsync(productId);
            return await DoTransactionAsync(basar, transactionType, notes, null, product);
        }
        public async Task<ProductsTransaction> DoTransactionAsync(Basar basar, TransactionType transactionType, string notes, PrintSettings printSettings, params Product[] products)
        {
            if (!products.IsAllowed(transactionType))
            {
                return null;
            }

            products.SetState(transactionType);
            await _db.SaveChangesAsync();

            return await CreateTransactionAsync(basar, transactionType, null, notes, printSettings, products);
        }
        public async Task<bool> ExistsAsync(int id) => await _db.Transactions.ExistsAsync(id);
        public async Task<bool> ExistsAsync(Basar basar, TransactionType type, int number) => await _db.Transactions.AnyAsync(t => t.BasarId == basar.Id && t.Type == type && t.Number == number);
        public IQueryable<ProductsTransaction> GetFromProduct(Basar basar, int productId)
        {
            return _db.Transactions.IncludeAll().Where(t => t.Products.Any(pt => pt.ProductId == productId)).OrderBy(t => t.TimeStamp);
        }
        public async Task<ProductsTransaction> GetLatestAsync(Basar basar, int productId)
        {
            return await GetFromProduct(basar, productId).OrderByDescending(t => t.TimeStamp).FirstAsync();
        }
        public async Task<ProductsTransaction> GetAsync(int id) => await IncludeAll().FirstOrDefaultAsync(t => t.Id == id);
        public async Task<ProductsTransaction> GetAsync(Basar basar, TransactionType type, int number)
        {
            return await _db.Transactions.IncludeAll().FirstOrDefaultAsync(t => t.BasarId == basar.Id && t.Type == type && t.Number == number);
        }
        public IQueryable<ProductsTransaction> GetMany(Basar basar, TransactionType type)
        {
            return GetMany(basar, type, t => true);
        }
        public IQueryable<ProductsTransaction> GetMany(Basar basar, TransactionType type, int sellerId)
        {
            return GetMany(basar, type, t => t.SellerId == sellerId);
        }
        public IQueryable<ProductsTransaction> GetMany(Basar basar, TransactionType type, string searchString)
        {
            if (int.TryParse(searchString, out int id))
            {
                return _db.Transactions.Where(t => t.Id == id);
            }
            else if (!string.IsNullOrEmpty(searchString))
            {
                return _db.Transactions.GetMany(type, basar, TransactionSearch(searchString));
            }

            return _db.Transactions.GetMany(type, basar);
        }
        public async Task RevertAsync(ProductsTransaction transaction)
        {
            Contract.Requires(transaction != null);

            if (transaction.Type == TransactionType.Settlement)
            {
                await _sellerContext.SetValueStateAsync(transaction.SellerId.Value, ValueState.NotSettled);

                foreach (var product in transaction.Products.GetProducts())
                {
                    product.ValueState = ValueState.NotSettled;
                    await _productContext.UpdateAsync(product);
                }
            }

            await DeleteAsync(transaction);
            await _productStatusService.PushAwayAsync(transaction.Basar, transaction.Products.GetProducts());
        }
        public async Task<ProductsTransaction> SettleSellerAsync(Basar basar, int sellerId)
        {
            var seller = await _sellerContext.GetAsync(sellerId);
            var sellersProducts = await _productContext.GetProductsForSeller(basar, seller.Id).ToArrayAsync();

            var productsToSettle = sellersProducts.Where(p => p.IsAllowed(TransactionType.Settlement)).ToArray();
            productsToSettle.SetState(TransactionType.Settlement);

            var printSettings = await _settingsContext.GetPrintSettingsAsync();

            var tx = await CreateTransactionAsync(basar, TransactionType.Settlement, seller, printSettings, productsToSettle);

            await _sellerContext.SetValueStateAsync(sellerId, ValueState.Settled);

            return tx;
        }
        public async Task UpdateProductAsync(Product product)
        {
            Contract.Requires(product != null);

            await _productContext.UpdateAsync(product);
            await UpdateTransactionDocumentsForProduct(product);
        }

        private async Task<ProductsTransaction> CreateTransactionAsync(Basar basar, TransactionType transactionType, PrintSettings printSettings, params Product[] products)
        {
            return await CreateTransactionAsync(basar, transactionType, null, null, printSettings, products);
        }
        private async Task<ProductsTransaction> CreateTransactionAsync(Basar basar, TransactionType transactionType, Seller seller, PrintSettings printSettings, params Product[] products)
        {
            return await CreateTransactionAsync(basar, transactionType, seller, null, printSettings, products);
        }
        private async Task<ProductsTransaction> CreateTransactionAsync(Basar basar, TransactionType transactionType, Seller seller, string notes, PrintSettings printSettings, params Product[] products)
        {
            var transaction = new ProductsTransaction
            {
                Basar = basar,
                Number = _numberContext.NextNumber(basar, transactionType),
                TimeStamp = DateTime.Now,
                Notes = notes,
                Type = transactionType,
                Seller = seller
            };
            transaction.Products = products.Select(p => new ProductToTransaction { Product = p, Transaction = transaction }).ToList();

            if (transaction.CanHasDocument())
            {
                transaction.DocumentId = await _fileContext.CreateTransactionDocumentAsync(transaction, printSettings);
            }

            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();

            if (transactionType == TransactionType.Acceptance || transactionType == TransactionType.Cancellation || transactionType == TransactionType.Sale || transactionType == TransactionType.Settlement)
            {
                await _productStatusService.PushAwayAsync(basar, products);
            }
            
            return transaction;
        }
        private async Task DeleteAsync(ProductsTransaction transaction)
        {
            if (transaction.DocumentId != null)
            {
                await _fileContext.DeleteAsync(transaction.DocumentId.Value);
            }
            _db.Transactions.Remove(transaction);
            await _db.SaveChangesAsync();
        }
        private IQueryable<ProductsTransaction> GetMany(Basar basar, TransactionType type, Expression<Func<ProductsTransaction, bool>> additionalPredicate = null)
        {
            var result = _db.Transactions.Where(t => t.Type == type && t.Basar.Id == basar.Id);

            if (additionalPredicate != null)
            {
                return IncludeAll(result.Where(additionalPredicate));
            }

            return IncludeAll(result);
        }
        private IQueryable<ProductsTransaction> IncludeAll() => IncludeAll(_db.Transactions);
        private async Task RemoveProductsFromTransactionAsync(ProductsTransaction transaction, IList<Product> productsToRemove)
        {
            foreach (var product in productsToRemove)
            {
                transaction.Products.Remove(transaction.Products.First(s => s.ProductId == product.Id));
            }
            if (transaction.Products.Count() > 0)
            {
                await _db.SaveChangesAsync();
                
                var printSettings = await _settingsContext.GetPrintSettingsAsync();
                await _fileContext.UpdateTransactionDocumentAsync(transaction, printSettings);
            }
            else
            {
                await DeleteAsync(transaction);
            }
        }
        private Expression<Func<ProductsTransaction, bool>> TransactionSearch(string searchString)
        {
            if (_db.IsPostgreSQL())
            {
                return x => (x.Seller == null) ? true :
                     EF.Functions.ILike(x.Seller.FirstName, $"%{searchString}%")
                  || EF.Functions.ILike(x.Seller.LastName, $"%{searchString}%")
                  || EF.Functions.ILike(x.Seller.City, $"%{searchString}%")
                  || EF.Functions.ILike(x.Seller.Country.Name, $"%{searchString}%")
                  || (x.Seller.BankAccountHolder != null && EF.Functions.ILike(x.Seller.BankAccountHolder, $"%{searchString}%"));
            }
            return x => (x.Seller == null) ? true :
                 EF.Functions.Like(x.Seller.FirstName, $"%{searchString}%")
              || EF.Functions.Like(x.Seller.LastName, $"%{searchString}%")
              || EF.Functions.Like(x.Seller.City, $"%{searchString}%")
              || EF.Functions.Like(x.Seller.Country.Name, $"%{searchString}%")
              || (x.Seller.BankAccountHolder != null && EF.Functions.Like(x.Seller.BankAccountHolder, $"%{searchString}%"));
        }
        private async Task UpdateTransactionDocumentsForProduct(Product product)
        {
            var printSettings = await _settingsContext.GetPrintSettingsAsync();
            var transactions = await GetFromProduct(product.Basar, product.Id).ToArrayAsync();
            foreach (var transaction in  transactions.Where(t => t.CanHasDocument()))
            {
                await _fileContext.UpdateTransactionDocumentAsync(transaction, printSettings);
            }
        }

        private static IQueryable<ProductsTransaction> IncludeAll(IQueryable<ProductsTransaction> transactions)
        {
            return transactions
                .Include(t => t.Basar)
                .Include(t => t.Seller)
                    .ThenInclude(s => s.Country)
                .Include(t => t.Products)
                    .ThenInclude(pt => pt.Product)
                        .ThenInclude(p => p.Brand)
                .Include(t => t.Products)
                    .ThenInclude(pt => pt.Product)
                        .ThenInclude(p => p.Type);
        }
    }
}
