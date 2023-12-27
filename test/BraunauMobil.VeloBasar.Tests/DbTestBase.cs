using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Tests.Mockups;

namespace BraunauMobil.VeloBasar.Tests;

public class DbTestBase<TDbFixture>
    : IDisposable
    where TDbFixture : IDbFixture, new()
{
    private readonly TDbFixture _dbFixture = new();

    public virtual void Dispose()
    {
        _dbFixture.Dispose();
        GC.SuppressFinalize(this);
    }

    public VeloDbContext Db { get => _dbFixture.Db; }

    public ClockMock Clock { get => _dbFixture.Clock; }
}
