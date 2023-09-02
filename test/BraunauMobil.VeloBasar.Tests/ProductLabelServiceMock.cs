using BraunauMobil.VeloBasar.Pdf;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests;

public class ProductLabelServiceMock
    : IProductLabelService
{
    public async Task<byte[]> CreateLabelAsync(ProductEntity product)
    {
        //  @todo Add better implementation
        byte[] data = Array.Empty<byte>();
        return await Task.FromResult(data);
    }

    public async Task<byte[]> CreateLabelsAsync(IEnumerable<ProductEntity> products)
    {
        //  @todo Add better implementation
        byte[] data = Array.Empty<byte>();
        return await Task.FromResult(data);
    }
}
