using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptProductControllerTests;

public class Create
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task WithId_ReturnsView(int sessionId)
    {
        //  Arrange
        AcceptProductModel model = Fixture.Create<AcceptProductModel>();
        AcceptProductService.Setup(_ => _.CreateNewAsync(sessionId))
            .ReturnsAsync(model);

        //  Act
        IActionResult result = await Sut.Create(sessionId);

        //  Assert
        result.Should().NotBeNull();
        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().Be(model);
        viewResult.ViewName.Should().Be("CreateEdit");
        viewResult.ViewData.ModelState.ErrorCount.Should().Be(0);

        AcceptProductService.Verify(_ => _.CreateNewAsync(sessionId));
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task WithValidEntity_CallsCreate_And_ReturnsRedirectToCreate(int sessionId, string url)
    {
        //  Arrange
        ProductEntity entity = Fixture.Create<ProductEntity>();
        entity.SessionId = sessionId;
        AcceptProductRouter.Setup(_ => _.ToCreate(sessionId))
            .Returns(url);

        //  Act
        IActionResult result = await Sut.Create(entity);

        //  Assert
        result.Should().NotBeNull();
        RedirectResult redirectResult = result.Should().BeOfType<RedirectResult>().Subject;
        redirectResult.Url.Should().Be(url);

        AcceptProductService.Verify(_ => _.CreateAsync(entity), Times.Once());
        Router.Verify(_ => _.AcceptProduct, Times.Once());
        AcceptProductRouter.Verify(_ => _.ToCreate(sessionId));
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task WithInvalidEntity_CallsGet_And_Returns_View(int sessionId, string url)
    {
        //  Arrange
        ProductEntity entity = Fixture.Create<ProductEntity>();
        entity.Price = 0;
        entity.SessionId = sessionId;
        AcceptProductModel acceptProductModel = Fixture.Create<AcceptProductModel>();
        AcceptProductService.Setup(_ => _.GetAsync(sessionId, entity))
            .ReturnsAsync(acceptProductModel);
        AcceptProductRouter.Setup(_ => _.ToCreate(sessionId))
            .Returns(url);

        //  Act
        IActionResult result = await Sut.Create(entity);

        //  Assert
        result.Should().NotBeNull();
        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().Be(acceptProductModel);
        viewResult.ViewData.ModelState.ErrorCount.Should().Be(1);
        viewResult.ViewName.Should().Be("CreateEdit");

        AcceptProductService.Verify(_ => _.GetAsync(sessionId, entity), Times.Once);
        VerifyNoOtherCalls();
    }
}
