using BraunauMobil.VeloBasar.Parameters;
using Microsoft.AspNetCore.Mvc;
using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptSessionControllerTests;

public class List
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task WithPageSizeNull_ThrowsArgumentNullException(AcceptSessionListParameter parameter)
    {
        //  Arrange
        parameter.PageSize = null;

        //  Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await Sut.List(parameter));
    }

    [Theory]
    [VeloAutoData]
    public async Task CallsGetAll_And_ReturnsView(AcceptSessionListParameter parameter)
    {
        ArgumentNullException.ThrowIfNull(parameter.PageSize);

        //  Arrange
        A.CallTo(() => AcceptSessionService.GetAllAsync(parameter.PageSize.Value, parameter.PageIndex, parameter.BasarId, parameter.AcceptSessionState)).Returns(Helpers.EmptyPaginatedList<AcceptSessionEntity>());

        //  Act
        IActionResult result = await Sut.List(parameter);

        //  Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.Model.Should().BeOfType<ListModel<AcceptSessionEntity, AcceptSessionListParameter>>();
            view.ViewData.ModelState.IsValid.Should().BeTrue();
        }

        A.CallTo(() => AcceptSessionService.GetAllAsync(parameter.PageSize.Value, parameter.PageIndex, parameter.BasarId, parameter.AcceptSessionState)).MustHaveHappenedOnceExactly();
    }
}