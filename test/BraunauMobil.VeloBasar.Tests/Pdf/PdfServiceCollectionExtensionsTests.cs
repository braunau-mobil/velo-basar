using BraunauMobil.VeloBasar.Pdf;
using Microsoft.Extensions.DependencyInjection;

namespace BraunauMobil.VeloBasar.Tests.Pdf;

public class PdfServiceCollectionExtensionsTests
{
    [Fact]
    public void AllServicesShouldbeRegistered()
    {
        //  Arrange
        IServiceCollection services = new ServiceCollection();

        //  Act
        services.AddPdf();

        //  Assert
        services.Should().SatisfyRespectively(
            X.CreateInspector<PdfGenerator, PdfGenerator>(ServiceLifetime.Singleton),
            X.CreateInspector<IProductLabelGenerator, ItextProductLabelGenerator>(ServiceLifetime.Scoped),
            X.CreateInspector<ITransactionDocumentGenerator, ItextTransactionDocumentGenerator>(ServiceLifetime.Scoped)
            );
    }
}
