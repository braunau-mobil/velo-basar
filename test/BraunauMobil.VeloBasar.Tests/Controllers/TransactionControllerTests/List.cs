using System.Security.Claims;
using BraunauMobil.VeloBasar.Parameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.Controllers.TransactionControllerTests;

public class List
	: TestBase
{
	[Theory]
	[VeloAutoData]
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
		A.CallTo(() => Router.ToLogin()).Returns(url);

		//	Act
		IActionResult result = await Sut.List(parameter, activeBasarId);

        //	Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
		    redirect.Url.Should().Be(url);
        }

		A.CallTo(() => Router.ToLogin()).MustHaveHappenedOnceExactly();
	}

    [Theory]
    [VeloAutoData]
    public async Task NotSaleAndSignedIn_CallsGetManyAndReturnsView(TransactionListParameter parameter, int activeBasarId)
    {
		//	Arrange
		IEnumerable<TransactionEntity> transactions = Fixture.CreateMany<TransactionEntity>();
        Sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal()
            }
        };
        parameter.TransactionType = TransactionType.Acceptance;
		SignInManager.IsSignedInMock = principal => true;
		A.CallTo(() => TransactionService.GetManyAsync(parameter.PageSize!.Value, parameter.PageIndex, activeBasarId, parameter.TransactionType, parameter.SearchString)).Returns(Fixture.CreatePaginatedList(transactions.ToList()));

        //	Act
        IActionResult result = await Sut.List(parameter, activeBasarId);

        //	Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
		    view.Model.Should().BeOfType<ListModel<TransactionEntity, TransactionListParameter>>();
		    view.ViewData.ModelState.IsValid.Should().BeTrue();
        }

        A.CallTo(() => TransactionService.GetManyAsync(parameter.PageSize!.Value, parameter.PageIndex, activeBasarId, parameter.TransactionType, parameter.SearchString)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task SaleAndSignedIn_CallsGetManyAndReturnsView(TransactionListParameter parameter, int activeBasarId)
    {
        //	Arrange
        IEnumerable<TransactionEntity> transactions = Fixture.CreateMany<TransactionEntity>();
        Sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal()
            }
        };
        parameter.TransactionType = TransactionType.Sale;
        SignInManager.IsSignedInMock = principal => true;
        A.CallTo(() => TransactionService.GetManyAsync(parameter.PageSize!.Value, parameter.PageIndex, activeBasarId, parameter.TransactionType, parameter.SearchString)).Returns(Fixture.CreatePaginatedList(transactions.ToList()));

        //	Act
        IActionResult result = await Sut.List(parameter, activeBasarId);

        //	Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.Model.Should().BeOfType<ListModel<TransactionEntity, TransactionListParameter>>();
            view.ViewData.ModelState.IsValid.Should().BeTrue();
        }

        A.CallTo(() => TransactionService.GetManyAsync(parameter.PageSize!.Value, parameter.PageIndex, activeBasarId, parameter.TransactionType, parameter.SearchString)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task SaleAndNotSignedIn_CallsGetManyAndReturnsView(TransactionListParameter parameter, int activeBasarId)
    {
        //	Arrange
        IEnumerable<TransactionEntity> transactions = Fixture.CreateMany<TransactionEntity>();
        Sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal()
            }
        };
        parameter.TransactionType = TransactionType.Sale;
        SignInManager.IsSignedInMock = principal => false;
        A.CallTo(() => TransactionService.GetManyAsync(parameter.PageSize!.Value, parameter.PageIndex, activeBasarId, parameter.TransactionType, parameter.SearchString)).Returns(Fixture.CreatePaginatedList(transactions.ToList()));

        //	Act
        IActionResult result = await Sut.List(parameter, activeBasarId);

        //	Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.Model.Should().BeOfType<ListModel<TransactionEntity, TransactionListParameter>>();
            view.ViewData.ModelState.IsValid.Should().BeTrue();
        }

        A.CallTo(() => TransactionService.GetManyAsync(parameter.PageSize!.Value, parameter.PageIndex, activeBasarId, parameter.TransactionType, parameter.SearchString)).MustHaveHappenedOnceExactly();
    }
}

