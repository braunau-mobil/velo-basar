using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Configuration;
using Microsoft.Extensions.Options;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AdminServiceTests;

public class TestBase
    : DbTestBase
{
    public TestBase()
    {
        IOptions<ExportSettings> exportOptions = X.StrictFake<IOptions<ExportSettings>>();
        A.CallTo(() => exportOptions.Value).Returns(ExportSettings);

        A.CallTo(() => DataGeneratorService.Contextualize(A<DataGeneratorConfiguration>._)).DoesNothing();

        Sut = new AdminService(DocumentService, TokenProvider, DataGeneratorService, exportOptions, Db);
    }

    protected IDocumentService DocumentService { get; } = X.StrictFake<IDocumentService>();

    protected ExportSettings ExportSettings { get; } = new ExportSettings();

    protected IDataGeneratorService DataGeneratorService { get; } = X.StrictFake<IDataGeneratorService>();

    protected AdminService Sut { get; }

    protected ITokenProvider TokenProvider { get; } = X.StrictFake<ITokenProvider>();
}
