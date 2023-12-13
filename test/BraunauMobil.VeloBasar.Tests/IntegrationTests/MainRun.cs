using BraunauMobil.VeloBasar.Tests.IntegrationTests.MainRunSteps;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests;

public class MainRun
    : TestBase
{
    [Fact]
    public async Task Run()
    {
        await InitDatabase();
    }

    private async Task InitDatabase()
    {
        await InitalSetup.Run(Services);
        await BasarCreation.Run(Services);
    }   
}
