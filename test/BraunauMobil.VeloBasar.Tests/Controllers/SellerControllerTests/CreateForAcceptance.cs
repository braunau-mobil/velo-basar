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
		A.CallTo(() => SellerService.CreateNewAsync()).Returns(seller);

		//	Act
		IActionResult result = await Sut.CreateForAcceptance(id);

		//	Assert
		using (new AssertionScope())
		{
			ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
			view.ViewData.ModelState.IsValid.Should().BeTrue();
			SellerCreateForAcceptanceModel model = view.Model.Should().BeOfType<SellerCreateForAcceptanceModel>().Subject;
			model.Seller.Should().Be(seller);
		}

		A.CallTo(() => SellerService.CreateNewAsync()).MustHaveHappenedOnceExactly();
	}


    [Theory]
    [AutoData]
    public async Task WithId_CallsGetAsyncAndReturnsView(int id, SellerEntity seller)
    {
        //	Arrange
        A.CallTo(() => SellerService.GetAsync(id)).Returns(seller);

        //	Act
        IActionResult result = await Sut.CreateForAcceptance(id);

        //	Assert
        using (new AssertionScope())
		{
			ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
			view.ViewData.ModelState.IsValid.Should().BeTrue();
			SellerCreateForAcceptanceModel model = view.Model.Should().BeOfType<SellerCreateForAcceptanceModel>().Subject;
			model.Seller.Should().Be(seller);
		}

        A.CallTo(() => SellerService.GetAsync(id)).MustHaveHappenedOnceExactly();
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
        using (new AssertionScope())
		{
			ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
			view.ViewData.ModelState.IsValid.Should().BeFalse();
			SellerCreateForAcceptanceModel model = view.Model.Should().BeOfType<SellerCreateForAcceptanceModel>().Subject;
			model.Seller.Should().Be(seller);
		}
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
		A.CallTo(() => SellerService.UpdateAsync(seller)).DoesNothing();
		A.CallTo(() => AcceptSessionRouter.ToStartForSeller(seller.Id)).Returns(url);

		//	Act
		IActionResult result = await Sut.CreateForAcceptance(seller);

        //	Assert
        using (new AssertionScope())
		{
			RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
			redirect.Url.Should().Be(url);
		}

		A.CallTo(() => AcceptSessionRouter.ToStartForSeller(seller.Id)).MustHaveHappenedOnceExactly();
		A.CallTo(() => SellerService.UpdateAsync(seller)).MustHaveHappenedOnceExactly();
	}

    [Theory]
    [AutoData]
    public async Task WithValidModelThatNotExists_CallsCreateAndReturnsRedirectToStartSession(SellerEntity seller, string url, int id)
    {
        //	Arrange
		seller.Id = 0;
        seller.IBAN = null;
        seller.HasNewsletterPermission = false;
        seller.EMail = null;
        seller.PhoneNumber = "123";
        A.CallTo(() => SellerService.CreateAsync(seller)).Returns(id);
		A.CallTo(() => AcceptSessionRouter.ToStartForSeller(seller.Id)).Returns(url);


        //	Act
        IActionResult result = await Sut.CreateForAcceptance(seller);

		//	Assert
		using (new AssertionScope())
		{
			RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
			redirect.Url.Should().Be(url);
		}

        A.CallTo(() => AcceptSessionRouter.ToStartForSeller(seller.Id)).MustHaveHappenedOnceExactly();
        A.CallTo(() => SellerService.CreateAsync(seller)).MustHaveHappenedOnceExactly();
    }
}
