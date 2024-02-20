namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptSessionServiceTests;

public class IsSessionRunning
    : TestBase
{
    [Fact]
    public async Task Null_ReturnsFalse()
    {
        //  Arrange

        //  Act
        bool isRunning = await Sut.IsSessionRunning(null);

        //  Assert
        isRunning.Should().BeFalse();
    }

    [Theory]
    [VeloAutoData]
    public async Task NonExistent_ReturnsFalse(int sessionId)
    {
        // Arrange

        //  Act
        bool isRunning = await Sut.IsSessionRunning(sessionId);

        //  Assert
        isRunning.Should().BeFalse();
    }

    [Theory]
    [VeloAutoData]
    public async Task EndTimeStampSet_ReturnsFalse(AcceptSessionEntity session, DateTime endTimeStamp)
    {
        // Arrange
        Db.AcceptSessions.Add(session);
        session.EndTimeStamp = endTimeStamp;
        await Db.SaveChangesAsync();

        //  Act
        bool isRunning = await Sut.IsSessionRunning(session.Id);

        //  Assert
        isRunning.Should().BeFalse();
    }

    [Theory]
    [VeloAutoData]
    public async Task NoEndTimeStamp_ReturnsTrue(AcceptSessionEntity session)
    {
        // Arrange
        Db.AcceptSessions.Add(session);
        session.EndTimeStamp = null;
        await Db.SaveChangesAsync();

        //  Act
        bool isRunning = await Sut.IsSessionRunning(session.Id);

        //  Assert
        isRunning.Should().BeTrue();
    }
}
