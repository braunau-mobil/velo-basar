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
            X.CreateInspector<IProductLabelService, ProductLabelService>(ServiceLifetime.Scoped),
            X.CreateInspector<ITransactionDocumentService, TransactionDocumentService>(ServiceLifetime.Scoped)
            );
    }
}
