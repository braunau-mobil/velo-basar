﻿using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.BasarControllerTests;

public class ActiveBasarDetails
    : TestBase
{
    [Theory]
    [AutoData]
    public void RedirectsToBasarDetails(int activeBasarId, string url)
    {
        //  Arrange
        BasarRouter.Setup(_ => _.GetUriByAction(nameof(BasarController.Details), It.IsAny<object>()))
            .Returns(url);

        //  Act
        IActionResult result = Sut.ActiveBasarDetails(activeBasarId);

        //  Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        BasarRouter.Verify(_ => _.GetUriByAction(nameof(BasarController.Details), It.IsAny<object>()), Times.Once());
        VerifyNoOtherCalls();
    }
}