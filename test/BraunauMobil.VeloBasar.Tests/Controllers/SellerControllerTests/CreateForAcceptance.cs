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
}

