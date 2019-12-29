using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Printing;
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
        private const string PdfContentType = "application/pdf";

        private readonly VeloRepository _db;
        private readonly INumberContext _numberContext;
        private readonly IPrintService _printService;
        private readonly IProductContext _productContext;
        private readonly ISettingsContext _settingsContext;
        private readonly IFileStoreContext _fileContext;
        private readonly ISellerContext _sellerContext;

        public TransactionContext(VeloRepository db, INumberContext numberContext, IPrintService printService, IProductContext productContext, ISettingsContext settingsContext, IFileStoreContext fileContext, ISellerContext sellerContext)
        {
            _db = db;
            _numberContext = numberContext;
            _printService = printService;
            _productContext = productContext;
            _settingsContext = settingsContext;
            _fileContext = fileContext;
            _sellerContext = sellerContext;
        }

        public async Task<ProductsTransaction> AcceptProductsAsync(Basar basar, int sellerId, IList<Product> products)
        {
            Contract.Requires(products != null);
            
            var seller = await _sellerContext.GetAsync(sellerId);
            var printSettings = await _settingsContext.GetPrintSettingsAsync();
            var productsInserted = await InsertProductsAsync(products);

            var tx = await CreateTransactionAsync(basar, TransactionType.Acceptance, seller, printSettings, productsInserted.ToArray());
            await GenerateTransactionDocumentAsync(tx, printSettings);

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
        public async Task<FileData> CreateLabelsForAcceptanceAsync(Basar basar, int acceptanceNumber)
        {
            var acceptance = await GetAsync(basar, TransactionType.Acceptance, acceptanceNumber);
            return await CreateLabelsAndCombineToOnePdfAsync(basar, acceptance.Products.Select(pt => pt.Product));
        }
        public async Task<FileData> CreateLabelsForSellerAsync(Basar basar, int sellerId)
        {
            var products = await _productContext.GetProductsForSeller(basar, sellerId).Where(p => p.Label != null).ToListAsync();
            return await CreateLabelsAndCombineToOnePdfAsync(basar, products);
        }
        public async Task<ProductsTransaction> DoTransactionAsync(Basar basar, TransactionType transactionType, string notes, int productId)
        {
            var product = await _productContext.GetAsync(productId);
            var printSettings = await _settingsContext.GetPrintSettingsAsync();
            return await DoTransactionAsync(basar, transactionType, notes, printSettings, product);
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
        public async Task<int> GetTransactionNumberForProductAsync(Basar basar, TransactionType type, int productId)
        {
            var transaction = await _db.Transactions.AsNoTracking().Include(t => t.Products).Where(t => t.Type == type && t.BasarId == basar.Id).FirstOrDefaultAsync(t => t.Products.Any(pt => pt.ProductId == productId));
            return transaction.Number;
        }
        public async Task<ProductsTransaction> SettleSellerAsync(Basar basar, int sellerId)
        {
            var seller = await _sellerContext.GetAsync(sellerId);
            var sellersProducts = await _productContext.GetProductsForSeller(basar, seller.Id).ToListAsync();

            var products = sellersProducts.Where(p => p.IsAllowed(TransactionType.Settlement));
            products.SetState(TransactionType.Settlement);

            var printSettings = await _settingsContext.GetPrintSettingsAsync();

            return await CreateTransactionAsync(basar, TransactionType.Settlement, seller, printSettings, products.ToArray());
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
            var tx = new ProductsTransaction
            {
                Basar = basar,
                Number = _numberContext.NextNumber(basar, transactionType),
                TimeStamp = DateTime.Now,
                Notes = notes,
                Type = transactionType,
                Seller = seller
            };
            tx.Products = products.Select(p => new ProductToTransaction { Product = p, Transaction = tx }).ToList();
            _db.Transactions.Add(tx);
            await _db.SaveChangesAsync();

            await GenerateTransactionDocumentAsync(tx, printSettings);

            return tx;
        }
        private async Task<FileData> CreateLabelsAndCombineToOnePdfAsync(Basar basar, IEnumerable<Product> products)
        {
            var printSettings = await _settingsContext.GetPrintSettingsAsync();

            var files = new List<byte[]>();
            foreach (var product in products)
            {
                var file = await CreateLabelAsync(basar, product, printSettings);
                files.Add(file.Data);
            }

            if (files.Count <= 0)
            {
                return null;
            }

            return new FileData
            {
                Data = _printService.Combine(files),
                ContentType = PdfContentType
            };
        }
        private async Task<FileData> CreateLabelAsync(Basar basar, Product product, PrintSettings printSettings)
        {
            var fileStore = new FileData
            {
                ContentType = PdfContentType,
                Data = _printService.CreateLabel(basar, product, printSettings)
            };
            _db.Files.Add(fileStore);
            await _db.SaveChangesAsync();

            product.Label = fileStore.Id;

            await _db.SaveChangesAsync();

            return fileStore;
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
        private async Task<IDictionary<Product, Seller>> GetProductToSellerMapAsync(ProductsTransaction transaction)
        {
            var productIds = transaction.Products.Select(x => x.ProductId).ToArray();
            var result = new Dictionary<Product, Seller>();
            foreach (var product in transaction.Products)
            {
                var acceptances = await _db.Transactions.GetMany(TransactionType.Acceptance, transaction.Basar, tx => tx.Products.Any(pt => pt.ProductId == product.ProductId)).ToArrayAsync();
                var seller = acceptances.First().Seller;
                result.Add(product.Product, seller);
            }
            return result;
        }
        private async Task<FileData> GenerateTransactionDocumentAsync(ProductsTransaction transaction, PrintSettings printSettings)
        {
            FileData fileStore;
            if (transaction.DocumentId == null)
            {
                fileStore = new FileData
                {
                    ContentType = PdfContentType
                };
                _db.Files.Add(fileStore);
                await _db.SaveChangesAsync();

                transaction.DocumentId = fileStore.Id;
            }
            else
            {
                fileStore = await _fileContext.GetAsync(transaction.DocumentId.Value);
            }

            if (transaction.Type == TransactionType.Acceptance)
            {
                fileStore.Data = _printService.CreateAcceptance(transaction, printSettings);
            }
            else if (transaction.Type == TransactionType.Sale)
            {
                fileStore.Data = _printService.CreateSale(transaction, await GetProductToSellerMapAsync(transaction), printSettings);
            }
            else if (transaction.Type == TransactionType.Settlement)
            {
                fileStore.Data = _printService.CreateSettlement(transaction, printSettings);
            }

            await _db.SaveChangesAsync();

            return fileStore;
        }
        private IQueryable<ProductsTransaction> IncludeAll() => IncludeAll(_db.Transactions);
        private async Task<IList<Product>> InsertProductsAsync(IList<Product> products)
        {
            Contract.Requires(products != null);

            var newProducts = new List<Product>();
            foreach (var product in products)
            {
                var newProduct = new Product
                {
                    Brand = product.Brand,
                    Color = product.Color,
                    Description = product.Description,
                    FrameNumber = product.FrameNumber,
                    Label = product.Label,
                    Price = product.Price,
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
        private async Task RemoveProductsFromTransactionAsync(ProductsTransaction transaction, IList<Product> productsToRemove)
        {
            foreach (var product in productsToRemove)
            {
                transaction.Products.Remove(transaction.Products.First(s => s.ProductId == product.Id));
            }
            await _db.SaveChangesAsync();
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
        private static Expression<Func<ProductsTransaction, bool>> TransactionSearch(string searchString)
        {
            return x => (x.Seller == null) ? true :
                 x.Seller.FirstName.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
              || x.Seller.LastName.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
              || x.Seller.City.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
              || x.Seller.Country.Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
              || (x.Seller.BankAccountHolder != null && x.Seller.BankAccountHolder.Contains(searchString, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
