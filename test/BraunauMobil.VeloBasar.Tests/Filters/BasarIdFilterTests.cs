using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Extensions;
using BraunauMobil.VeloBasar.Filters;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace BraunauMobil.VeloBasar.Tests.Filters;

public class BasarIdFilterTests
{
    private class MyModel
        : IHasBasarId
    {
        public int BasarId { get; set; }
    }

    private readonly BasarIdFilter _sut;
    private readonly ActionExecutingContext _executingContext;
    private readonly ActionExecutionDelegate _actionExecutionDelegate;
    private readonly Controller _controller;
    private readonly IBasarRouter _router = X.StrictFake<IBasarRouter>();
    private readonly IBasarService _basarService = X.StrictFake<IBasarService>();

    public BasarIdFilterTests()
    {
        _sut = new(_router, _basarService);

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
    public async Task NoBasarId_ActionArgumentHasNoId_ModelHasNoId_ShouldOnlyCallNext()
    {
        //  Arrange
        A.CallTo(() => _basarService.GetActiveBasarIdAsync()).Returns<int?>(null);

        //  Act
        await _sut.OnActionExecutionAsync(_executingContext, _actionExecutionDelegate);

        //  Assert
        _controller.ViewData.GetActiveBasar().Should().Be(null);
        A.CallTo(() => _basarService.GetActiveBasarIdAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => _actionExecutionDelegate.Invoke()).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task NoBasarId_ActionParameterHasId_ModelHasNoId_ShouldRedirectToBasarList(string url)
    {
        //  Arrange
        A.CallTo(() => _basarService.GetActiveBasarIdAsync()).Returns<int?>(null);
        A.CallTo(() => _router.ToList()).Returns(url);
        _executingContext.ActionDescriptor.Parameters = new List<ParameterDescriptor> { new() { Name = BasarIdFilter.BasarIdArgumentName } };

        //  Act
        await _sut.OnActionExecutionAsync(_executingContext, _actionExecutionDelegate);

        //  Assert
        using (new AssertionScope())
        {
            _controller.ViewData.GetActiveBasar().Should().Be(null);
            _executingContext.ActionArguments.Should().NotContainKey(BasarIdFilter.BasarIdArgumentName);
            RedirectResult redirect =  _executingContext.Result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }
        A.CallTo(() => _basarService.GetActiveBasarIdAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => _router.ToList()).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task NoBasarId_ActionParameterHasNoId_ModelHasId_ShouldRedirectToBasarList(string url)
    {
        //  Arrange
        A.CallTo(() => _basarService.GetActiveBasarIdAsync()).Returns<int?>(null);
        A.CallTo(() => _router.ToList()).Returns(url);
        MyModel myModel = new();
        _executingContext.ActionArguments.Add(nameof(myModel), myModel);

        //  Act
        await _sut.OnActionExecutionAsync(_executingContext, _actionExecutionDelegate);

        //  Assert
        using (new AssertionScope())
        {
            _controller.ViewData.GetActiveBasar().Should().Be(null);
            _executingContext.ActionArguments[nameof(myModel)].Should().Be(myModel);
            myModel.BasarId.Should().Be(0);
            RedirectResult redirect = _executingContext.Result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }
        A.CallTo(() => _basarService.GetActiveBasarIdAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => _router.ToList()).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task BasarId_ActionArgumentHasNoId_ModelHasNoId_ShouldSetBasarOnViewData_And_CallNext(BasarEntity basar)
    {
        //  Arrange
        A.CallTo(() => _basarService.GetActiveBasarIdAsync()).Returns(basar.Id);
        A.CallTo(() => _basarService.GetAsync(basar.Id)).Returns(basar);

        //  Act
        await _sut.OnActionExecutionAsync(_executingContext, _actionExecutionDelegate);

        //  Assert
        _controller.ViewData.GetActiveBasar().Should().Be(basar);
        A.CallTo(() => _basarService.GetActiveBasarIdAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => _basarService.GetAsync(basar.Id)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _actionExecutionDelegate.Invoke()).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task BasarId_ActionParameterHasId_ModelHasNoId_ShouldSetParameter_BasarOnViewData_And_CallNext(BasarEntity basar)
    {
        //  Arrange
        A.CallTo(() => _basarService.GetActiveBasarIdAsync()).Returns(basar.Id);
        A.CallTo(() => _basarService.GetAsync(basar.Id)).Returns(basar);
        _executingContext.ActionDescriptor.Parameters = new List<ParameterDescriptor> { new() { Name = BasarIdFilter.BasarIdArgumentName } };

        //  Act
        await _sut.OnActionExecutionAsync(_executingContext, _actionExecutionDelegate);

        //  Assert
        using (new AssertionScope())
        {
            _controller.ViewData.GetActiveBasar().Should().Be(basar);
            _executingContext.ActionArguments[BasarIdFilter.BasarIdArgumentName].Should().Be(basar.Id);
        }
        A.CallTo(() => _basarService.GetActiveBasarIdAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => _basarService.GetAsync(basar.Id)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _actionExecutionDelegate.Invoke()).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task BasarId_ActionParameterHasNoId_ModelHasId_ShouldSetParameter_IdOnModel_BasarOnViewData_And_CallNext(BasarEntity basar)
    {
        //  Arrange
        A.CallTo(() => _basarService.GetActiveBasarIdAsync()).Returns(basar.Id);
        A.CallTo(() => _basarService.GetAsync(basar.Id)).Returns(basar);
        MyModel myModel = new();
        _executingContext.ActionArguments.Add(nameof(myModel), myModel);

        //  Act
        await _sut.OnActionExecutionAsync(_executingContext, _actionExecutionDelegate);

        //  Assert
        using (new AssertionScope())
        {
            _controller.ViewData.GetActiveBasar().Should().Be(basar);
            myModel.BasarId.Should().Be(basar.Id);
        }
        A.CallTo(() => _basarService.GetActiveBasarIdAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => _basarService.GetAsync(basar.Id)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _actionExecutionDelegate.Invoke()).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task BasarId_ActionParameterHasId_ModelHasId_ShouldSetParameter_IdOnModel_BasarOnViewData_And_CallNext(BasarEntity basar)
    {
        //  Arrange
        A.CallTo(() => _basarService.GetActiveBasarIdAsync()).Returns(basar.Id);
        A.CallTo(() => _basarService.GetAsync(basar.Id)).Returns(basar);
        _executingContext.ActionDescriptor.Parameters = new List<ParameterDescriptor> { new() { Name = BasarIdFilter.BasarIdArgumentName } };
        MyModel myModel = new();
        _executingContext.ActionArguments.Add(nameof(myModel), myModel);

        //  Act
        await _sut.OnActionExecutionAsync(_executingContext, _actionExecutionDelegate);

        //  Assert
        using (new AssertionScope())
        {
            _controller.ViewData.GetActiveBasar().Should().Be(basar);
            _executingContext.ActionArguments[BasarIdFilter.BasarIdArgumentName].Should().Be(basar.Id);
            myModel.BasarId.Should().Be(basar.Id);
        }
        A.CallTo(() => _basarService.GetActiveBasarIdAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => _basarService.GetAsync(basar.Id)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _actionExecutionDelegate.Invoke()).MustHaveHappenedOnceExactly();
    }
}
