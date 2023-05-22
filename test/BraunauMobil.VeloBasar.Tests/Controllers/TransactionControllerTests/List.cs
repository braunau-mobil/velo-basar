using System.Collections.Generic;
using System.Security.Claims;
using BraunauMobil.VeloBasar.Parameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xan.AspNetCore.Models;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.Controllers.TransactionControllerTests;

public class List
	: TestBase
{
	[Theory]
	[AutoData]
	public async Task NotSaleAndNotSignedIn_ReturnsRedirectToLogin(TransactionListParameter parameter,int activeBasarId, string url)
	{
		//	Arrange
		Sut.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext
			{
				User = new ClaimsPrincipal()
			}
		};
		parameter.TransactionType = TransactionType.Acceptance;
		SignInManager.IsSignedInMock = principal => false;
		Router.Setup(_ => _.ToLogin())
			.Returns(url);

		//	Act
		IActionResult result = await Sut.List(parameter, activeBasarId);

		//	Assert
		RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
		redirect.Url.Should().Be(url);

		Router.Verify(_ => _.ToLogin(), Times.Once());
		VerifyNoOtherCalls();
	}

    [Theory]
    [AutoData]
    public async Task NotSaleAndSignedIn_CallsGetManyAndReturnsView(TransactionListParameter parameter, int activeBasarId)
    {
		//	Arrange
		IEnumerable<TransactionEntity> transactions = Fixture.BuildTransaction().CreateMany();
        Sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal()
            }
        };
        parameter.TransactionType = TransactionType.Acceptance;
		SignInManager.IsSignedInMock = principal => true;
		TransactionService.Setup(_ => _.GetManyAsync(parameter.PageSize!.Value, parameter.PageIndex, activeBasarId, parameter.TransactionType, parameter.SearchString))
			.ReturnsAsync(Fixture.CreatePaginatedList(transactions.ToList()));

        //	Act
        IActionResult result = await Sut.List(parameter, activeBasarId);

		//	Assert
		ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
		view.Model.Should().BeOfType<ListModel<TransactionEntity, TransactionListParameter>>();
		view.ViewData.ModelState.IsValid.Should().BeTrue();

        TransactionService.Verify(_ => _.GetManyAsync(parameter.PageSize!.Value, parameter.PageIndex, activeBasarId, parameter.TransactionType, parameter.SearchString), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task SaleAndSignedIn_CallsGetManyAndReturnsView(TransactionListParameter parameter, int activeBasarId)
    {
        //	Arrange
        IEnumerable<TransactionEntity> transactions = Fixture.BuildTransaction().CreateMany();
        Sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal()
            }
        };
        parameter.TransactionType = TransactionType.Sale;
        SignInManager.IsSignedInMock = principal => true;
        TransactionService.Setup(_ => _.GetManyAsync(parameter.PageSize!.Value, parameter.PageIndex, activeBasarId, parameter.TransactionType, parameter.SearchString))
            .ReturnsAsync(Fixture.CreatePaginatedList(transactions.ToList()));

        //	Act
        IActionResult result = await Sut.List(parameter, activeBasarId);

        //	Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().BeOfType<ListModel<TransactionEntity, TransactionListParameter>>();
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        TransactionService.Verify(_ => _.GetManyAsync(parameter.PageSize!.Value, parameter.PageIndex, activeBasarId, parameter.TransactionType, parameter.SearchString), Times.Once());
        VerifyNoOtherCalls();
    }

    [Theory]
    [AutoData]
    public async Task SaleAndNotSignedIn_CallsGetManyAndReturnsView(TransactionListParameter parameter, int activeBasarId)
    {
        //	Arrange
        IEnumerable<TransactionEntity> transactions = Fixture.BuildTransaction().CreateMany();
        Sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal()
            }
        };
        parameter.TransactionType = TransactionType.Sale;
        SignInManager.IsSignedInMock = principal => false;
        TransactionService.Setup(_ => _.GetManyAsync(parameter.PageSize!.Value, parameter.PageIndex, activeBasarId, parameter.TransactionType, parameter.SearchString))
            .ReturnsAsync(Fixture.CreatePaginatedList(transactions.ToList()));

        //	Act
        IActionResult result = await Sut.List(parameter, activeBasarId);

        //	Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.Model.Should().BeOfType<ListModel<TransactionEntity, TransactionListParameter>>();
        view.ViewData.ModelState.IsValid.Should().BeTrue();

        TransactionService.Verify(_ => _.GetManyAsync(parameter.PageSize!.Value, parameter.PageIndex, activeBasarId, parameter.TransactionType, parameter.SearchString), Times.Once());
        VerifyNoOtherCalls();
    }
}

