using BraunauMobil.VeloBasar.BusinessLogic;
using Microsoft.Extensions.DependencyInjection;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarServiceTests;

[Collection(TestCollections.SampleDatabase)]
public class GetBasarNameAsync
{
    private readonly SampleDatabaseFixture _db;

    public GetBasarNameAsync(SampleDatabaseFixture db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    [Fact]
    public async Task NonExistentBasar_Throws()
    {
        //  Arrange
        IBasarService sut = _db.ServiceScope.ServiceProvider.GetRequiredService<IBasarService>();

        //  Act
        Func<Task> act = async () => await sut.GetBasarNameAsync(666);

        //  Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task ExistingBasar_ShouldReturnName()
    {
        //  Arrange
        IBasarService sut = _db.ServiceScope.ServiceProvider.GetRequiredService<IBasarService>();

        //  Act
        string name = await sut.GetBasarNameAsync(1);

        //  Assert
        name.Should().Be("1. Fahrradbasar");
    }
}
