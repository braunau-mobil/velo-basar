namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptSessionServiceTests;

public class SubmitAsync
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task SessionIsCompletedAndAcceptIsCalled(AcceptSessionEntity session, DateTime endTimeStamp, int acceptanceId)
    {
        // Arrange
        session.State = AcceptSessionState.Uncompleted;
        session.EndTimeStamp = null;
        Db.AcceptSessions.Add(session);
        await Db.SaveChangesAsync();
        A.CallTo(() => TransactionService.AcceptAsync(session.BasarId, session.SellerId, Enumerable.Empty<int>()))
            .Returns(acceptanceId);
        Clock.Now = endTimeStamp;


        //  Act
        int result= await Sut.SubmitAsync(session.Id);

        //  Assert
        using (new AssertionScope())
        {
            result.Should().Be(acceptanceId);
            session.State.Should().Be(AcceptSessionState.Completed);
            session.EndTimeStamp.Should().Be(endTimeStamp);
        }
        A.CallTo(() => TransactionService.AcceptAsync(session.BasarId, session.SellerId, Enumerable.Empty<int>())).MustHaveHappenedOnceExactly();
    }
}
