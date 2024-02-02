using BraunauMobil.VeloBasar.Models.Documents;

namespace BraunauMobil.VeloBasar.Pdf;

public interface IProductLabelGenerator
{
    Task<byte[]> CreateLabelAsync(ProductLabelDocumentModel model);

    Task<byte[]> CreateLabelsAsync(IEnumerable<ProductLabelDocumentModel> models);
}
