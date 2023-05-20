using System;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SellerControllerTests;

public class CreateForAcceptance
	: TestBase
{
	[Theory]
	[AutoData]
	public async Task NoId_CallsCreateNewAndReturnsView(SellerEntity seller)
	{
		//	Arrange
		int? id = null;
		SellerService.Setup(_ => _.CreateNewAsync())
			.ReturnsAsync(seller);

		//	Act
		IActionResult result = await Sut.CreateForAcceptance(id);

		//	Assert
		ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
		view.ViewData.ModelState.ErrorCount.Should().Be(0);
		SellerCreateForAcceptanceModel model = view.Model.Should().BeOfType<SellerCreateForAcceptanceModel>().Subject;
		model.Seller.Should().Be(seller);

		SellerService.Verify(_ => _.CreateNewAsync(), Times.Once());
		VerifyNoOtherCalls();
	}


    [Theory]
    [AutoData]
    public async Task WithId_CallsGetAsyncAndReturnsView(int id, SellerEntity seller)
    {
        //	Arrange
        SellerService.Setup(_ => _.GetAsync(id))
            .ReturnsAsync(seller);

        //	Act
        IActionResult result = await Sut.CreateForAcceptance(id);

        //	Assert
        ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
        view.ViewData.ModelState.ErrorCount.Should().Be(0);
        SellerCreateForAcceptanceModel model = view.Model.Should().BeOfType<SellerCreateForAcceptanceModel>().Subject;
        model.Seller.Should().Be(seller);

        SellerService.Verify(_ => _.GetAsync(id), Times.Once());
        VerifyNoOtherCalls();
    }

	[Theory]
	[AutoData]
	public async Task WithInvalidModel_ReturnsView(SellerEntity seller)
	{
		//	Arrange
		seller.FirstName = "";

		//	Act
		IActionResult result = await Sut.CreateForAcceptance(seller);

		//	Assert
		ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
		view.ViewData.ModelState.ErrorCount.Should().NotBe(0);
		SellerCreateForAcceptanceModel model = view.Model.Should().BeOfType<SellerCreateForAcceptanceModel>().Subject;
		model.Seller.Should().Be(seller);

		VerifyNoOtherCalls();
	}

	[Theory]
	[AutoData]
	public async Task WithValidModelThatExists_CallsUpdateAndReturnsRedirectToStartSession(SellerEntity seller, string url)
	{
		//	Arrange
		seller.IBAN = null;
		seller.HasNewsletterPermission = false;
		seller.EMail = null;
		seller.PhoneNumber = "123";
		SellerService.Setup(_ => _.UpdateAsync(seller));
		AcceptSessionRouter.Setup(_ => _.ToStartForSeller(seller.Id))
			.Returns(url);

		//	Act
		IActionResult result = await Sut.CreateForAcceptance(seller);

		//	Assert
		RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
		redirect.Url.Should().Be(url);

		AcceptSessionRouter.Verify(_ => _.ToStartForSeller(seller.Id), Times.Once());
		SellerService.Verify(_ => _.UpdateAsync(seller), Times.Once());
		VerifyNoOtherCalls();
	}

    [Theory]
    [AutoData]
    public async Task WithValidModelThatNotExists_CallsCreateAndReturnsRedirectToStartSession(SellerEntity seller, string url)
    {
        //	Arrange
		seller.Id = 0;
        seller.IBAN = null;
        seller.HasNewsletterPermission = false;
        seller.EMail = null;
        seller.PhoneNumber = "123";
        SellerService.Setup(_ => _.CreateAsync(seller));
        AcceptSessionRouter.Setup(_ => _.ToStartForSeller(seller.Id))
            .Returns(url);

        //	Act
        IActionResult result = await Sut.CreateForAcceptance(seller);

        //	Assert
        RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
        redirect.Url.Should().Be(url);

        AcceptSessionRouter.Verify(_ => _.ToStartForSeller(seller.Id), Times.Once());
        SellerService.Verify(_ => _.CreateAsync(seller), Times.Once());
        VerifyNoOtherCalls();
    }
}
