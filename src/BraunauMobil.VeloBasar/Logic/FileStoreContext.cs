using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Printing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public class FileStoreContext : IFileStoreContext
    {
        private const string PdfContentType = "application/pdf";

        private readonly VeloRepository _db;
        private readonly IPrintService _printService;

        public FileStoreContext(VeloRepository db, IPrintService printService)
        {
            _db = db;
            _printService = printService;
        }
        public async Task<int> CreateProductLabelAsync(Product product, PrintSettings printSettings)
        {
            var fileData = new FileData
            {
                ContentType = PdfContentType,
                Data = _printService.CreateLabel(product, printSettings)
            };
            _db.Files.Add(fileData);
            await _db.SaveChangesAsync();

            return fileData.Id;
        }
        public async Task<int> CreateTransactionDocumentAsync(ProductsTransaction transaction, PrintSettings printSettings)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var fileData = new FileData
            {
                ContentType = PdfContentType,
                Data = _printService.CreateTransaction(transaction, printSettings)
            };

            _db.Files.Add(fileData);
            await _db.SaveChangesAsync();

            return fileData.Id;
        }
        public Task<bool> ExistsAsync(int id) => _db.Files.ExistsAsync(id);
        public async Task DeleteAsync(int id)
        {
            var file = await GetAsync(id);
            if (file != null)
            {
                _db.Files.Remove(file);
                await _db.SaveChangesAsync();
            }
        }
        public async Task<FileData> GetAsync(int id)
        {
            return await _db.Files.FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<FileData> GetProductLabelsAndCombineToOnePdfAsync(IEnumerable<Product> products)
        {
            if (products == null) throw new ArgumentNullException(nameof(products));

            var files = new List<byte[]>();
            foreach (var product in products)
            {
                var file = await GetAsync(product.LabelId);
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
        public async Task UpdateProductLabelAsync(Product product, PrintSettings printSettings)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            var file = await GetAsync(product.LabelId);

            file.Data = _printService.CreateLabel(product, printSettings);

            await UpdateAsync(file);
        }
        public async Task UpdateTransactionDocumentAsync(ProductsTransaction transaction, PrintSettings printSettings)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var file = await GetAsync(transaction.DocumentId.Value);

            file.Data = _printService.CreateTransaction(transaction, printSettings);

            await UpdateAsync(file);
        }

        private async Task UpdateAsync(FileData file)
        {
            _db.Attach(file).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
    }
}
