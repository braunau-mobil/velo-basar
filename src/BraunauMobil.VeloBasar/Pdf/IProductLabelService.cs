namespace BraunauMobil.VeloBasar.Pdf;

public interface IProductLabelService
{
    Task<byte[]> CreateLabelAsync(ProductEntity product);

    Task<byte[]> CreateLabelsAsync(IEnumerable<ProductEntity> products);
}
