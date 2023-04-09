using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptProductControllerTests
{
    public class Create
        : TestBase
    {
        [Theory]
        [AutoData]
        public async Task Id_ReturnsView(int sessionIS)
        {
            //  Arrange
            AcceptProductModel model = Fixture.Create<AcceptProductModel>();
            AcceptProductService.Setup(_ => _.CreateNewAsync(sessionIS))
                .ReturnsAsync(model);

            //  Act
            IActionResult result = await Sut.Create(sessionIS);

            //  Assert
            result.Should().NotBeNull();
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().Be(model);
            viewResult.ViewName.Should().Be("CreateEdit");
            viewResult.ViewData.ModelState.ErrorCount.Should().Be(0);

            AcceptProductService.Verify(_ => _.CreateNewAsync(sessionIS));
            VerifyNoOtherCalls();
        }

        [Theory]
        [AutoData]
        public async Task ValidEntity_Returns_RedirectToCreate(int sessionId, string url)
        {
            //  Arrange
            ProductEntity model = Fixture.Create<ProductEntity>();
            model.SessionId = sessionId;
            AcceptProductRouter.Setup(_ => _.ToCreate(sessionId))
                .Returns(url);

            //  Act
            IActionResult result = await Sut.Create(model);

            //  Assert
            result.Should().NotBeNull();
            RedirectResult redirectResult = result.Should().BeOfType<RedirectResult>().Subject;
            redirectResult.Url.Should().Be(url);

            AcceptProductService.Verify(_ => _.CreateAsync(model), Times.Once());
            Router.Verify(_ => _.AcceptProduct, Times.Once());
            AcceptProductRouter.Verify(_ => _.ToCreate(sessionId));
            VerifyNoOtherCalls();
        }
    }
}
