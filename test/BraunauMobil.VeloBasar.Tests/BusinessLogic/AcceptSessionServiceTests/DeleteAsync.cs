namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptSessionServiceTests;

public class DeleteAsync
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [AutoData]
    public async Task NonExistentSession_ThrowsInvalidOperationException(int sessionId)
    {
        //  Arrange

        //  Act
        Func<Task> act = async () => await Sut.DeleteAsync(sessionId);

        //  Assert
        await act.Should().ThrowAsync<InvalidOperationException>();

        VerifyNoOtherCalls();
    }


    [Theory]
    [AutoData]
    public async Task SessionIsDeleted(AcceptSessionEntity session)
    {
        // Arrange
        Db.AcceptSessions.Add(session);
        await Db.SaveChangesAsync();

        // Act
        await Sut.DeleteAsync(session.Id);

        // Assert
        Db.AcceptSessions.Should().BeEmpty();

        VerifyNoOtherCalls();
    }
}
