using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Extensions;
using BraunauMobil.VeloBasar.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace BraunauMobil.VeloBasar.Tests.Filters;

public class ActiveSessionIdFilterTests
    : DbTestBase<EmptySqliteDbFixture>
{
    private readonly ActiveSessionIdFilter _sut;
    private readonly IActiveAcceptSessionCookie _activeAcceptSessionCookie;
    private readonly ActionExecutingContext _executingContext;
    private readonly ActionExecutionDelegate _actionExecutionDelegate;
    private readonly Controller _controller;

    public ActiveSessionIdFilterTests()
    {
        _activeAcceptSessionCookie = X.StrictFake<IActiveAcceptSessionCookie>();
        _sut = new(_activeAcceptSessionCookie, Db);

        ActionContext actionContext = new(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
        IList<IFilterMetadata> filters = X.StrictFake<IList<IFilterMetadata>>();
        IDictionary<string, object?> arguments = new Dictionary<string, object?>();
        _controller = X.StrictFake<Controller>();

        _executingContext = new(actionContext, filters, arguments, _controller);
        ActionExecutedContext executedContext = new(actionContext, filters, _controller);

        _actionExecutionDelegate = A.Fake<ActionExecutionDelegate>();
        A.CallTo(() => _actionExecutionDelegate.Invoke()).Returns(executedContext);
    }

    [Fact]
    public async Task NoCokieSet_ShouldCallOnlyNext()
    {
        //  Arrange
        A.CallTo(() => _activeAcceptSessionCookie.GetActiveAcceptSessionId()).Returns(null);

        //  Act
        await _sut.OnActionExecutionAsync(_executingContext, _actionExecutionDelegate);

        //  Assert
        A.CallTo(() => _actionExecutionDelegate.Invoke()).MustHaveHappenedOnceExactly();
        _controller.ViewData.GetActiveSessionId().Should().BeNull();
    }

    [Theory]
    [VeloAutoData]
    public async Task CokieSet_NoSessionInDb_ShouldClearCookieAndCallNext(int acceptSessionId)
    {
        //  Arrange
        A.CallTo(() => _activeAcceptSessionCookie.GetActiveAcceptSessionId()).Returns(acceptSessionId);
        A.CallTo(() => _activeAcceptSessionCookie.ClearActiveAcceptSession()).DoesNothing();

        //  Act
        await _sut.OnActionExecutionAsync(_executingContext, _actionExecutionDelegate);

        //  Assert
        A.CallTo(() => _actionExecutionDelegate.Invoke()).MustHaveHappenedOnceExactly();
        A.CallTo(() => _activeAcceptSessionCookie.ClearActiveAcceptSession()).MustHaveHappenedOnceExactly();
        _controller.ViewData.GetActiveSessionId().Should().BeNull();
    }

    [Theory]
    [VeloAutoData]
    public async Task CokieSet_SessionInDb_IsCompleted_ShouldClearCookieAndCallNext(AcceptSessionEntity acceptSession)
    {
        //  Arrange
        Db.AcceptSessions.Add(acceptSession);
        await Db.SaveChangesAsync();
        A.CallTo(() => _activeAcceptSessionCookie.GetActiveAcceptSessionId()).Returns(acceptSession.Id);
        A.CallTo(() => _activeAcceptSessionCookie.ClearActiveAcceptSession()).DoesNothing();

        //  Act
        await _sut.OnActionExecutionAsync(_executingContext, _actionExecutionDelegate);

        //  Assert
        A.CallTo(() => _actionExecutionDelegate.Invoke()).MustHaveHappenedOnceExactly();
        A.CallTo(() => _activeAcceptSessionCookie.ClearActiveAcceptSession()).MustHaveHappenedOnceExactly();
        _controller.ViewData.GetActiveSessionId().Should().BeNull();
    }

    [Theory]
    [VeloAutoData]
    public async Task CokieSet_SessionInDb_IsNotCompleted_ShouldSetViewDataAndCallNext(AcceptSessionEntity acceptSession)
    {
        //  Arrange
        acceptSession.EndTimeStamp = null;
        Db.AcceptSessions.Add(acceptSession);
        await Db.SaveChangesAsync();
        A.CallTo(() => _activeAcceptSessionCookie.GetActiveAcceptSessionId()).Returns(acceptSession.Id);

        //  Act
        await _sut.OnActionExecutionAsync(_executingContext, _actionExecutionDelegate);

        //  Assert
        A.CallTo(() => _actionExecutionDelegate.Invoke()).MustHaveHappenedOnceExactly();
        _controller.ViewData.GetActiveSessionId().Should().Be(acceptSession.Id);
    }
}
