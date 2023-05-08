using BraunauMobil.VeloBasar.Parameters;
using Microsoft.AspNetCore.Mvc;
using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.Controllers.AcceptSessionControllerTests;

public class List
    : TestBase
{
    [Theory]
    [AutoData]
    public async Task WithPageSizeNull_ThrowsArgumentNullException(AcceptSessionListParameter parameter, int activeBasarId)
    {
        //  Arrange
        parameter.PageSize = null;

        //  Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await Sut.List(parameter, activeBasarId));
        
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task CallsGetAll_And_ReturnsView(AcceptSessionListParameter parameter, int activeBasarId)
    {
        ArgumentNullException.ThrowIfNull(parameter.PageSize);

        //  Arrange
        AcceptSessionService.Setup(_ => _.GetAllAsync(parameter.PageSize.Value, parameter.PageIndex, activeBasarId, parameter.AcceptSessionState))
            .ReturnsAsync(Helpers.EmptyPaginatedList<AcceptSessionEntity>());

        //  Act
        IActionResult result = await Sut.List(parameter, activeBasarId);

        //  Assert
        result.Should().NotBeNull();
        ViewResult viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.Model.Should().BeOfType<ListModel<AcceptSessionEntity, AcceptSessionListParameter>>();
        viewResult.ViewData.ModelState.ErrorCount.Should().Be(0);

        AcceptSessionService.Verify(_ => _.GetAllAsync(parameter.PageSize.Value, parameter.PageIndex, activeBasarId, parameter.AcceptSessionState), Times.Once());
        VerifyNoOtherCalls();
    }
}