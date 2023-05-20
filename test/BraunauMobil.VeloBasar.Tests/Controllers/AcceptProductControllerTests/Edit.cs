using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptProductControllerTests;

public class Edit
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task WithId_CallsGet_And_ReturnsView(int productId)
    {
        AcceptProductModel model = Fixture.Create<AcceptProductModel>();
        AcceptProductService.Setup(_ => _.GetAsync(productId))
            .ReturnsAsync(model);

        //  Act
        IActionResult result = await Sut.Edit(productId);

        //  Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().Be(model);
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        AcceptProductService.Verify(_ => _.GetAsync(productId), Times.Once);
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task WithValidEntity_CallsUpdate_And_ReturnsRedirectToCreate(int sessionId, string url)
    {
        //  Arrange
        ProductEntity entity = Fixture.Create<ProductEntity>();
        entity.SessionId = sessionId;
        AcceptProductRouter.Setup(_ => _.ToCreate(sessionId))
            .Returns(url);

        //  Act
        IActionResult result = await Sut.Edit(entity);

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        AcceptProductService.Verify(_ => _.UpdateAsync(entity), Times.Once());
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
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().Be(acceptProductModel);
        view.ViewData.ModelState.IsValid.Should().BeFalse();
        view.ViewName.Should().Be("CreateEdit");

        AcceptProductService.Verify(_ => _.GetAsync(sessionId, entity), Times.Once);
        VerifyNoOtherCalls();
    }
}
