using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.QrImportServiceTests;

public class TestBase
    : DbTestBase
{
    public TestBase()
    {
        Sut = new QrImportService(Db);
    }

    public VeloFixture Fixture { get; } = new();

    public QrImportService Sut { get; }
}
