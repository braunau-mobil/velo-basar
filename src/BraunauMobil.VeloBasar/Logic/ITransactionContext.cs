using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface ITransactionContext
    {
        Task<ProductsTransaction> AcceptProductsAsync(Basar basar, int sellerId, IReadOnlyList<Product> products);
        Task<ProductsTransaction> CancelProductsAsync(Basar basar, int saleId, IList<int> productIds);
        Task<ProductsTransaction> CheckoutProductsAsync(Basar basar, IList<int> productIds);
        Task<ProductsTransaction> DoTransactionAsync(Basar basar, TransactionType transactionType, string notes, int productId);
        Task<ProductsTransaction> DoTransactionAsync(Basar basar, TransactionType transactionType, string notes, PrintSettings printSettings, IReadOnlyCollection<Product> products);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsAsync(Basar basar, TransactionType type, int number);
        Task<ProductsTransaction> GetAsync(int id);
        Task<ProductsTransaction> GetAsync(Basar basar, TransactionType type, int number);
        IQueryable<ProductsTransaction> GetFromProduct(Basar basar, int productId);
        Task<ProductsTransaction> GetLatestAsync(Basar basar, int productId);
        IQueryable<ProductsTransaction> GetMany(Basar basar, TransactionType type);
        IQueryable<ProductsTransaction> GetMany(Basar basar, TransactionType type , int sellerId);
        IQueryable<ProductsTransaction> GetMany(Basar basar, TransactionType type, string searchString);
        Task RevertAsync(ProductsTransaction transaction);
        Task<ProductsTransaction> SettleSellerAsync(Basar basar, int sellerId);
        Task UpdateProductAsync(Product product);
    }
}
