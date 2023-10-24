using BraunauMobil.VeloBasar.BusinessLogic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarServiceTests;

public class GetBasarNameAsync
    : TestBase<SampleSqliteDbFixture>
{
    [Fact]
    public async Task NonExistentBasar_Throws()
    {
        //  Arrange
        Mock<IColorProvider> colorProvider = new ();
        BasarService sut = new(Db, colorProvider.Object);

        //  Act
        Func<Task> act = async () => await sut.GetBasarNameAsync(666);

        //  Assert
        await act.Should().ThrowAsync<InvalidOperationException>();

        VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ExistingBasar_ShouldReturnName()
    {
        //  Arrange
        Mock<IColorProvider> colorProvider = new();
        BasarService sut = new(Db, colorProvider.Object);

        //  Act
        string name = await sut.GetBasarNameAsync(1);

        //  Assert
        name.Should().Be("1. Fahrradbasar");

        VerifyNoOtherCalls();
    }
}
