using BraunauMobil.VeloBasar.Data;
using Xan.Extensions;

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

    public Mock<IClock> Clock { get; } = new();
}
