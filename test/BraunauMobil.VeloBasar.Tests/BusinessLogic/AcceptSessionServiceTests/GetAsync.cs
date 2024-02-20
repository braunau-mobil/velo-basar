namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptSessionServiceTests;

public class GetAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task NotExistenet_ThrowsInvalidOperationException(int sessionId)
    {
        //  Arrange
        
        //  Act
        Func<Task> act = async () => await Sut.GetAsync(sessionId);

        //  Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Theory]
    [VeloAutoData]
    public async Task IsReturnedAndRelationsAreLoaded(AcceptSessionEntity session)
    {
        // Arrange
        Db.AcceptSessions.Add(session);
        await Db.SaveChangesAsync();

        // Act
        AcceptSessionEntity result = await Sut.GetAsync(session.Id);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().Be(session);
        }
    }
}
