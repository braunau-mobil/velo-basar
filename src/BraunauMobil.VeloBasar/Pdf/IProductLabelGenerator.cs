using BraunauMobil.VeloBasar.Configuration;
using BraunauMobil.VeloBasar.Models.Documents;

namespace BraunauMobil.VeloBasar.Pdf;

public interface IProductLabelGenerator
{
    Task<byte[]> CreateLabelAsync(ProductLabelDocumentModel model, LabelPrintSettings settings);

    Task<byte[]> CreateLabelsAsync(IEnumerable<ProductLabelDocumentModel> models, LabelPrintSettings settings);
}
