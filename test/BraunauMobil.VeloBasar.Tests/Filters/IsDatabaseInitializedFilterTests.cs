using BraunauMobil.VeloBasar.Filters;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace BraunauMobil.VeloBasar.Tests.Filters;

public class IsDatabaseInitializedFilterTests
{
    private readonly IsDatabaseInitializedFilter _sut;
    private readonly ActionExecutingContext _executingContext;
    private readonly ActionExecutionDelegate _actionExecutionDelegate;
    private readonly Controller _controller;
    private readonly IAppContext _appContext = X.StrictFake<IAppContext>();
    private readonly ISetupRouter _router = X.StrictFake<ISetupRouter>();

    public IsDatabaseInitializedFilterTests()
    {
        _sut = new(_appContext, _router, A.Fake<ILogger<IsDatabaseInitializedFilter>>());

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
    public async Task Initialized_ShouldNotRedirect()
    {
        //  Arrange
        A.CallTo(() => _appContext.IsDatabaseInitialized()).Returns(true);

        //  Act
        await _sut.OnActionExecutionAsync(_executingContext, _actionExecutionDelegate);

        //  Assert
        _executingContext.Result.Should().BeNull();

        A.CallTo(() => _appContext.IsDatabaseInitialized()).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task NotInitialized_ShouldRedirectToInitialSetup(string url)
    {
        //  Arrange
        A.CallTo(() => _appContext.IsDatabaseInitialized()).Returns(false);
        A.CallTo(() => _router.ToInitialSetup()).Returns(url);

        //  Act
        await _sut.OnActionExecutionAsync(_executingContext, _actionExecutionDelegate);

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = _executingContext.Result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }
        
        A.CallTo(() => _appContext.IsDatabaseInitialized()).MustHaveHappenedOnceExactly();
        A.CallTo(() => _router.ToInitialSetup()).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task NotInitializedResultAlreadySet_ShouldExecuteResultAndThenRedirectToInitialSetup(string url)
    {
        //  Arrange
        A.CallTo(() => _appContext.IsDatabaseInitialized()).Returns(false);
        A.CallTo(() => _router.ToInitialSetup()).Returns(url);

        IActionResult result = X.StrictFake<IActionResult>();
        _executingContext.Result = result;
        A.CallTo(() => result.ExecuteResultAsync(_executingContext)).DoesNothing();

        //  Act
        await _sut.OnActionExecutionAsync(_executingContext, _actionExecutionDelegate);

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = _executingContext.Result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => _appContext.IsDatabaseInitialized()).MustHaveHappenedOnceExactly();
        A.CallTo(() => _router.ToInitialSetup()).MustHaveHappenedOnceExactly();
        A.CallTo(() => result.ExecuteResultAsync(_executingContext)).MustHaveHappenedOnceExactly();
    }
}
