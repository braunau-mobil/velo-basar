﻿namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptSessionServiceTests;

public class FindAsync
    : TestBase<EmptySqliteDbFixture>
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

        VerifyNoOtherCalls();
    }


    [Theory]
    [AutoData]
    public async Task SessionIsFound(AcceptSessionEntity session)
    {
        // Arrange
        Db.AcceptSessions.Add(session);
        await Db.SaveChangesAsync();

        // Act
        AcceptSessionEntity? foundSession = await Sut.FindAsync(session.Id);

        // Assert
        foundSession.Should().Be(session);

        VerifyNoOtherCalls();
    }
}