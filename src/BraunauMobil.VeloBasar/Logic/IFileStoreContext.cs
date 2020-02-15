using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface IFileStoreContext
    {
        Task<int> CreateProductLabelAsync(Product product, PrintSettings printSettings);
        Task<int> CreateTransactionDocumentAsync(ProductsTransaction transaction, PrintSettings printSettings);
        Task<bool> ExistsAsync(int id);
        Task DeleteAsync(int id);
        Task<FileData> GetAsync(int id);
        Task<FileData> GetProductLabelsAndCombineToOnePdfAsync(IEnumerable<Product> products);
        Task UpdateProductLabelAsync(Product product, PrintSettings printSettingsprint);
        Task UpdateTransactionDocumentAsync(ProductsTransaction transaction, PrintSettings printSettings);
    }
}
