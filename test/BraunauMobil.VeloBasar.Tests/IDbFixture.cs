using BraunauMobil.VeloBasar.Data;

namespace BraunauMobil.VeloBasar.Tests;

public interface IDbFixture
    : IDisposable
{
    VeloDbContext Db { get; }

    ClockMock Clock { get; }
}
