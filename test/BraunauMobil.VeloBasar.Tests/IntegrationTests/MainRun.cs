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
        _context.Clock.Now = new DateTimeOffset(2063, 04, 05, 11, 22, 33, TimeSpan.Zero);

        await new InitalSetup(_context).Run();
        await new BasarCreation(_context).Run();
        await new AcceptSellers(_context).Run();
        await new SellProducts(_context).Run();
    }
}
