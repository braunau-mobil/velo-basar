using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Tests.Mockups;

namespace BraunauMobil.VeloBasar.Tests;

public interface IDbFixture
    : IDisposable
{
    VeloDbContext Db { get; }

    ClockMock Clock { get; }
}
