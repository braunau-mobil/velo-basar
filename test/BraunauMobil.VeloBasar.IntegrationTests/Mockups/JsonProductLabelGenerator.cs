using BraunauMobil.VeloBasar.Models.Documents;
using BraunauMobil.VeloBasar.Pdf;

namespace BraunauMobil.VeloBasar.IntegrationTests.Mockups;

public class JsonProductLabelGenerator
    : IProductLabelGenerator
{
    public async Task<byte[]> CreateLabelAsync(ProductLabelDocumentModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return await Task.FromResult(model.SerializeAsJson());
    }

    public async Task<byte[]> CreateLabelsAsync(IEnumerable<ProductLabelDocumentModel> models)
    {
        ArgumentNullException.ThrowIfNull(models);

        return await Task.FromResult(models.SerializeAsJson());
    }
}
