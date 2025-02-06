using BraunauMobil.VeloBasar.Models.Documents;
using BraunauMobil.VeloBasar.Pdf;
using System.Text;
using BraunauMobil.VeloBasar.Configuration;

namespace BraunauMobil.VeloBasar.DataGenerator.Mockups;

public class ProductLabelServiceMock
    : IProductLabelGenerator
{
    public async Task<byte[]> CreateLabelAsync(ProductLabelDocumentModel model, LabelPrintSettings settings)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(settings);

        byte[] bytes = Encoding.UTF8.GetBytes(model.Title);

        return await Task.FromResult(bytes);
    }

    public async Task<byte[]> CreateLabelsAsync(IEnumerable<ProductLabelDocumentModel> models, LabelPrintSettings settings)
    {
        ArgumentNullException.ThrowIfNull(models);
        ArgumentNullException.ThrowIfNull(settings);

        StringBuilder sb = new();
        foreach (ProductLabelDocumentModel model in models)
        {
            sb.Append(model.Title);
        }
        byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
        return await Task.FromResult(bytes);
    }
}
