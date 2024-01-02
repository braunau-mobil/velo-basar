using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptProductControllerTests;

public class Create
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task WithId_ReturnsView(int sessionId)
    {
        //  Arrange
        AcceptProductModel model = Fixture.Create<AcceptProductModel>();
        A.CallTo(() => AcceptProductService.CreateNewAsync(sessionId)).Returns(model);

        //  Act
        IActionResult result = await Sut.Create(sessionId);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.Model.Should().Be(model);
            view.ViewName.Should().Be("CreateEdit");
            view.ViewData.ModelState.IsValid.Should().BeTrue();
        }

        A.CallTo(() => AcceptProductService.CreateNewAsync(sessionId)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task WithValidEntity_CallsCreate_And_ReturnsRedirectToCreate(int sessionId, string url)
    {
        //  Arrange
        ProductEntity entity = Fixture.Build<ProductEntity>().Create();
        entity.SessionId = sessionId;
        A.CallTo(() => AcceptProductRouter.ToCreate(sessionId)).Returns(url);
        A.CallTo(() => AcceptProductService.CreateAsync(entity)).DoesNothing();

        //  Act
        IActionResult result = await Sut.Create(entity);

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => AcceptProductService.CreateAsync(entity)).MustHaveHappenedOnceExactly();
        A.CallTo(() => Router.AcceptProduct).MustHaveHappenedOnceExactly();
        A.CallTo(() => AcceptProductRouter.ToCreate(sessionId));
    }

    [Theory]
    [VeloAutoData]
    public async Task WithInvalidEntity_CallsGet_And_Returns_View(int sessionId, string url)
    {
        //  Arrange
        ProductEntity entity = Fixture.Create<ProductEntity>();
        entity.Price = 0;
        entity.SessionId = sessionId;
        AcceptProductModel acceptProductModel = Fixture.Create<AcceptProductModel>();
        A.CallTo(() => AcceptProductService.GetAsync(sessionId, entity)).Returns(acceptProductModel);
        A.CallTo(() => AcceptProductRouter.ToCreate(sessionId)).Returns(url);

        //  Act
        IActionResult result = await Sut.Create(entity);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.Model.Should().Be(acceptProductModel);
            view.ViewData.ModelState.IsValid.Should().BeFalse();
            view.ViewName.Should().Be("CreateEdit");
        }

        A.CallTo(() => AcceptProductService.GetAsync(sessionId, entity)).MustHaveHappenedOnceExactly();
    }
}
