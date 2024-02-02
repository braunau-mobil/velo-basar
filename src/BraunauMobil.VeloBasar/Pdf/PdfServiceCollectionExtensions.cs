using Microsoft.Extensions.DependencyInjection;

namespace BraunauMobil.VeloBasar.Pdf;

public static class PdfServiceCollectionExtensions
{
    public static IServiceCollection AddPdf(this IServiceCollection sc)
    {
        ArgumentNullException.ThrowIfNull(sc);

        return sc
            .AddSingleton<PdfGenerator>()
            .AddScoped<IProductLabelGenerator, ItextProductLabelGenerator>()
            .AddScoped<ITransactionDocumentGenerator, ItextTransactionDocumentGenerator>()
            ;
    }
}
