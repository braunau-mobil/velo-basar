using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Configuration;
using BraunauMobil.VeloBasar.Pdf;
using Microsoft.Extensions.Options;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AdminServiceTests;

public class TestBase
    : DbTestBase<EmptySqliteDbFixture>
{
    public TestBase()
    {
        IOptions<ExportSettings> exportOptions = X.StrictFake<IOptions<ExportSettings>>();
        A.CallTo(() => exportOptions.Value).Returns(ExportSettings);

        A.CallTo(() => DataGeneratorService.Contextualize(A<DataGeneratorConfiguration>._)).DoesNothing();

        Sut = new AdminService(ProductLabelService, TransactionDocumentService, DataGeneratorService, exportOptions, Db);
    }

    protected ExportSettings ExportSettings { get; } = new ExportSettings();

    protected IDataGeneratorService DataGeneratorService { get; } = X.StrictFake<IDataGeneratorService>();

    protected IProductLabelService ProductLabelService { get; } = X.StrictFake<IProductLabelService>();

    protected AdminService Sut { get; }

    protected ITransactionDocumentService TransactionDocumentService { get; } = X.StrictFake<ITransactionDocumentService>();
}
