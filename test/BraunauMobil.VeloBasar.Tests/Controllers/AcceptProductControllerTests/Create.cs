using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptProductControllerTests
{
    public class Create
        : TestBase
    {
        [Theory]
        [AutoData]
        public async Task Id_ReturnsView(int id)
        {
            //  Arrange
            AcceptProductModel model = Fixture.Create<AcceptProductModel>();
            AcceptProductService.Setup(_ => _.CreateNewAsync(id))
                .ReturnsAsync(model);

            //  Act
            IActionResult result = await Sut.Create(id);

            //  Assert
            result.Should().NotBeNull();
            ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().Be(model);
            viewResult.ViewName.Should().Be("CreateEdit");

            AcceptProductService.Verify(_ => _.CreateNewAsync(id));
            VerifyNoOtherCalls();
        }
    }
}
