using BraunauMobil.VeloBasar.Configuration;
using BraunauMobil.VeloBasar.Pdf;

namespace BraunauMobil.VeloBasar.IntegrationTests.Mockups;

public class JsonProductLabelGenerator
    : IProductLabelGenerator
{
    public async Task<byte[]> CreateLabelAsync(ProductLabelDocumentModel model, LabelPrintSettings settings)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(settings);

        return await Task.FromResult(model.SerializeAsJson());
    }

    public async Task<byte[]> CreateLabelsAsync(IEnumerable<ProductLabelDocumentModel> models, LabelPrintSettings settings)
    {
        ArgumentNullException.ThrowIfNull(models);
        ArgumentNullException.ThrowIfNull(settings);

        return await Task.FromResult(models.SerializeAsJson());
    }
}
