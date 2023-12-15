using BraunauMobil.VeloBasar.Tests.IntegrationTests.MainRunSteps;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests;

public sealed class MainRun
    : IDisposable
{
    private readonly TestContext _context = new ();

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task Run()
    {
        await InitalSetup.Run(_context);
        await BasarCreation.Run(_context);
        await AcceptSellers.Run(_context);
    }   
}
