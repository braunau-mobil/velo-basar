using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptSessionServiceTests;

public class GetAllAsync
    : TestBase
{
    private const int _pageSize = 5;

    private readonly VeloFixture _fixture = new ();

    [Theory]
    [VeloAutoData]
    public async Task NoSessions_ReturnsEmpty(int basarId, int pageIndex)
    {
        //  Arrange

        //  Act
        IPaginatedList<AcceptSessionEntity> sessions = await Sut.GetAllAsync(_pageSize, pageIndex, basarId, null);

        //  Assert
        sessions.Should().BeEmpty();
    }

    [Theory]
    [VeloAutoData]
    public async Task OnlyOnePage(BasarEntity basar)
    {
        // Arrange
        Db.Basars.Add(basar);
        await Db.SaveChangesAsync();
        IEnumerable<AcceptSessionEntity> sessions = _fixture.BuildAcceptSession(basar)
            .CreateMany(_pageSize);
        Db.AcceptSessions.AddRange(sessions);
        await Db.SaveChangesAsync();

        // Act
        IPaginatedList<AcceptSessionEntity> foundSessions = await Sut.GetAllAsync(_pageSize, 0, basar.Id, null);

        // Assert
        foundSessions.Should().HaveCount(_pageSize);
        foundSessions.TotalItemCount.Should().Be(_pageSize);
    }

    [Theory]
    [VeloAutoData]
    public async Task Uncompleted(BasarEntity basar)
    {
        // Arrange
        Db.Basars.Add(basar);
        await Db.SaveChangesAsync();
        IEnumerable<AcceptSessionEntity> uncompletedSessions = _fixture.BuildAcceptSession(basar)
            .With(s => s.State, AcceptSessionState.Uncompleted)
            .CreateMany(_pageSize);
        Db.AcceptSessions.AddRange(uncompletedSessions);
        IEnumerable<AcceptSessionEntity> completedSessions = _fixture.BuildAcceptSession(basar)
            .With(s => s.State, AcceptSessionState.Completed)
            .CreateMany(_pageSize);
        Db.AcceptSessions.AddRange(completedSessions);
        await Db.SaveChangesAsync();

        // Act
        IPaginatedList<AcceptSessionEntity> foundSessions = await Sut.GetAllAsync(_pageSize, 0, basar.Id, AcceptSessionState.Uncompleted);

        // Assert
        foundSessions.TotalItemCount.Should().Be(uncompletedSessions.Count());
    }
}
