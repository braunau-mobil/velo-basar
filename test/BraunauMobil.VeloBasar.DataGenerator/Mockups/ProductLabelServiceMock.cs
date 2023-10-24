using BraunauMobil.VeloBasar.Models.Entities;
using BraunauMobil.VeloBasar.Pdf;
using System.Globalization;
using System.Text;

namespace BraunauMobil.VeloBasar.DataGenerator.Mockups;

public class ProductLabelServiceMock
    : IProductLabelService
{
    public async Task<byte[]> CreateLabelAsync(ProductEntity product)
    {
        ArgumentNullException.ThrowIfNull(product);

        string idString = product.Id.ToString(CultureInfo.InvariantCulture);
        byte[] bytes = Encoding.UTF8.GetBytes(idString);

        return await Task.FromResult(bytes);
    }

    public async Task<byte[]> CreateLabelsAsync(IEnumerable<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        StringBuilder sb = new();
        foreach (ProductEntity product in products)
        {
            sb.Append(product.Id);
        }
        byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
        return await Task.FromResult(bytes);
    }
}
