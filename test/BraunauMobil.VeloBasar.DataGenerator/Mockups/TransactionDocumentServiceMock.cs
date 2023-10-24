using BraunauMobil.VeloBasar.Models.Entities;
using BraunauMobil.VeloBasar.Pdf;
using System.Globalization;
using System.Text;

namespace BraunauMobil.VeloBasar.DataGenerator.Mockups;

public class TransactionDocumentServiceMock
    : ITransactionDocumentService
{
    public async Task<byte[]> CreateAsync(TransactionEntity transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        string idString = transaction.Id.ToString(CultureInfo.InvariantCulture);
        byte[] bytes = Encoding.UTF8.GetBytes(idString);

        return await Task.FromResult(bytes);
    }
}
