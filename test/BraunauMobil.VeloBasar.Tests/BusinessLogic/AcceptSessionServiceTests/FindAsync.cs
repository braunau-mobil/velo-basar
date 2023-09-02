namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptSessionServiceTests;

public class FindAsync
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task NonExistentSession_ReturnsNull(int sessionId)
    {
        //  Arrange

        //  Act
        AcceptSessionEntity? foundSession = await Sut.FindAsync(sessionId);

        //  Assert
        foundSession.Should().BeNull();
    }


    [Theory]
    [AutoData]
    public async Task SessoinIsFound(AcceptSessionEntity session)
    {
        // Arrange
        Db.AcceptSessions.Add(session);
        await Db.SaveChangesAsync();

        // Act
        AcceptSessionEntity? foundSession = await Sut.FindAsync(session.Id);

        // Assert
        foundSession.Should().Be(session);
    }
}
