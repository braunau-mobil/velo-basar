using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Configuration;
using BraunauMobil.VeloBasar.Pdf;
using FakeItEasy;
using Microsoft.Extensions.Options;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AdminServiceTests;

public class TestBase
    : DbTestBase<EmptySqliteDbFixture>
{
    public TestBase()
    {
        IOptions<ExportSettings> exportOptions = A.Fake<IOptions<ExportSettings>>();
        A.CallTo(() => exportOptions.Value).Returns(ExportSettings);

        A.CallTo(() => DataGeneratorService.Contextualize(A<DataGeneratorConfiguration>._)).DoesNothing();

        Sut = new AdminService(ProductLabelService, TransactionDocumentService, DataGeneratorService, exportOptions, Db);
    }

    protected ExportSettings ExportSettings { get; } = new ExportSettings();

    protected IDataGeneratorService DataGeneratorService { get; } = A.Fake<IDataGeneratorService>();

    protected IProductLabelService ProductLabelService { get; } = A.Fake<IProductLabelService>();

    protected AdminService Sut { get; }

    protected ITransactionDocumentService TransactionDocumentService { get; } = A.Fake<ITransactionDocumentService>();
}
