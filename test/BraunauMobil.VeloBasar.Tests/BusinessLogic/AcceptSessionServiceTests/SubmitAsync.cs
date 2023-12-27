namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptSessionServiceTests;

public class SubmitAsync
    : TestBase<EmptySqliteDbFixture>
{
    [Theory]
    [AutoData]
    public async Task SessionIsCompletedAndAcceptIsCalled(AcceptSessionEntity session, DateTime endTimeStamp, int acceptanceId)
    {
        // Arrange
        session.State = AcceptSessionState.Uncompleted;
        session.EndTimeStamp = null;
        session.Seller.ValueState = ValueState.Settled;
        Db.AcceptSessions.Add(session);
        await Db.SaveChangesAsync();
        TransactionService.Setup(_ => _.AcceptAsync(session.BasarId, session.SellerId, Enumerable.Empty<int>()))
            .ReturnsAsync(acceptanceId);
        Clock.Now = endTimeStamp;


        //  Act
        int result= await Sut.SubmitAsync(session.Id);

        //  Assert
        result.Should().Be(acceptanceId);
        session.State.Should().Be(AcceptSessionState.Completed);
        session.EndTimeStamp.Should().Be(endTimeStamp);
        session.Seller.ValueState.Should().Be(ValueState.NotSettled);
        TransactionService.Verify(_ => _.AcceptAsync(session.BasarId, session.SellerId, Enumerable.Empty<int>()), Times.Once());
        VerifyNoOtherCalls();
    }
}
