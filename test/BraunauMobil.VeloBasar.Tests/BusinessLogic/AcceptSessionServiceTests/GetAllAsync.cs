using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.AcceptSessionServiceTests;

public class GetAllAsync
    : TestBase<EmptySqliteDbFixture>
{
    private const int _pageSize = 5;

    private readonly Fixture _fixture = new Fixture();

    [Theory]
    [AutoData]
    public async Task NoSessions_ReturnsEmpty(int basarId, int pageIndex)
    {
        //  Arrange

        //  Act
        IPaginatedList<AcceptSessionEntity> sessions = await Sut.GetAllAsync(_pageSize, pageIndex, basarId, null);

        //  Assert
        sessions.Should().BeEmpty();
    }

    [Theory]
    [AutoData]
    public async Task OnlyOnePage(BasarEntity basar)
    {
        // Arrange
        Db.Basars.Add(basar);
        await Db.SaveChangesAsync();
        IEnumerable<AcceptSessionEntity> sessions = _fixture.Build<AcceptSessionEntity>()
            .With(s => s.Basar, basar)
            .With(s => s.BasarId, basar.Id)
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
    [AutoData]
    public async Task Uncompleted(BasarEntity basar)
    {
        // Arrange
        Db.Basars.Add(basar);
        await Db.SaveChangesAsync();
        IEnumerable<AcceptSessionEntity> uncompletedSessions = _fixture.Build<AcceptSessionEntity>()
            .With(s => s.Basar, basar)
            .With(s => s.BasarId, basar.Id)
            .With(s => s.State, AcceptSessionState.Uncompleted)
            .CreateMany(_pageSize);
        Db.AcceptSessions.AddRange(uncompletedSessions);
        IEnumerable<AcceptSessionEntity> completedSessions = _fixture.Build<AcceptSessionEntity>()
            .With(s => s.Basar, basar)
            .With(s => s.BasarId, basar.Id)
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
