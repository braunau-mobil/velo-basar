using BraunauMobil.VeloBasar.Tests.IntegrationTests.MainRunSteps;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests;

public class MainRun
    : TestBase
{
    [Fact]
    public async Task Run()
    {
        await InitalSetup.Run(Services);
        await BasarCreation.Run(Services);
        await AcceptSellers.Run(Services);
    }   
}
