using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptProductControllerTests;

public class Edit
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task WithId_CallsGet_And_ReturnsView(int productId)
    {
        AcceptProductModel model = Fixture.Create<AcceptProductModel>();
        A.CallTo(() => AcceptProductService.GetAsync(productId)).Returns(model);

        //  Act
        IActionResult result = await Sut.Edit(productId);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.Model.Should().Be(model);
            view.ViewData.ModelState.IsValid.Should().BeTrue();
        }

        A.CallTo(() => AcceptProductService.GetAsync(productId)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task WithValidEntity_CallsUpdate_And_ReturnsRedirectToCreate(int sessionId, string url)
    {
        //  Arrange
        ProductEntity entity = Fixture.Build<ProductEntity>().Create();
        entity.SessionId = sessionId;
        A.CallTo(() => AcceptProductRouter.ToCreate(sessionId)).Returns(url);
        A.CallTo(() => AcceptProductService.UpdateAsync(entity)).DoesNothing();

        //  Act
        IActionResult result = await Sut.Edit(entity);

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);
        }

        A.CallTo(() => AcceptProductService.UpdateAsync(entity)).MustHaveHappenedOnceExactly();
        A.CallTo(() => Router.AcceptProduct).MustHaveHappenedOnceExactly();
        A.CallTo(() => AcceptProductRouter.ToCreate(sessionId)).MustHaveHappenedOnceExactly();
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
