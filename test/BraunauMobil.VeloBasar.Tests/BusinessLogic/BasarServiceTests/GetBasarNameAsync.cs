namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.BasarServiceTests;

public class GetBasarNameAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task ShouldReturnName(BasarEntity basar)
    {
        //  Arrange
        Db.Add(basar);
        await Db.SaveChangesAsync();

        //  Act
        string result = await Sut.GetBasarNameAsync(basar.Id);

        //  Assert
        result.Should().Be(basar.Name);
    }
}
