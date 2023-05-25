namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptProductServiceTests;

public class CreateNewAsync
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task UncompletedSession_ReturnsNewAcceptProductModel(AcceptSessionEntity acceptSession)
    {
        //  Arrange
        acceptSession.EndTimeStamp = null;
        Db.AcceptSessions.Add(acceptSession);
        await Db.SaveChangesAsync();

        //  Act
        AcceptProductModel result = await Sut.CreateNewAsync(acceptSession.Id);

        //  Assert
        result.Should().NotBeNull();
        result.Entity.Should().NotBeNull();
        result.Entity.SessionId.Should().Be(acceptSession.Id);
    }

    [Theory]
    [AutoData]
    public async Task CompletedSession_ThrowsInvalidOperationException(AcceptSessionEntity acceptSession)
    {
        //  Arrange
        Db.AcceptSessions.Add(acceptSession);
        await Db.SaveChangesAsync();

        //  Act
        Func<Task> act = async () => await Sut.CreateNewAsync(acceptSession.Id);

        //  Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
