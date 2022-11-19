using Microsoft.Extensions.DependencyInjection;

namespace BraunauMobil.VeloBasar.Pdf;

public static class PdfServiceCollectionExtensions
{
    public static IServiceCollection AddPdf(this IServiceCollection sc)
    {
        ArgumentNullException.ThrowIfNull(sc);

        return sc
            .AddSingleton<PdfGenerator>()
            .AddScoped<IProductLabelService, ProductLabelService>()
            .AddScoped<ITransactionDocumentService, TransactionDocumentService>()
            ;
    }
}
