using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SecurityControllerTests;

public class Login_Post
    : TestBase
{
    [Theory]
    [VeloAutoData]
    public async Task InvalidModel_ShouldReturnViewWithValidationErrors(LoginModel model, ValidationFailure failure)
    {
        //  Arrange
        A.CallTo(() => Validator.ValidateAsync(model, default))
            .Returns(new ValidationResult()
            {
                Errors = [ failure]
            });

        //  Act
        IActionResult result = await Sut.Login(model);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.Model.Should().Be(model);
            view.ViewData.ModelState.IsValid.Should().BeFalse();
            view.ViewData.ModelState.Should().ContainKey(failure.PropertyName);
        }
        A.CallTo(() => Validator.ValidateAsync(model, default)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task ValidModel_ShouldLogin_LoginFails_ShouldReturnViewWithShowErrorSet(LoginModel model)
    {
        //  Arrange
        A.CallTo(() => Validator.ValidateAsync(model, default))
            .Returns(new ValidationResult());
        SignInManager.PasswordSignInAsyncResult = Microsoft.AspNetCore.Identity.SignInResult.Failed;

        //  Act
        IActionResult result = await Sut.Login(model);

        //  Assert
        using (new AssertionScope())
        {
            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.Model.Should().Be(model);
            view.ViewData.ModelState.IsValid.Should().BeTrue();
            model.ShowErrorMessage.Should().BeTrue();
        }
        A.CallTo(() => Validator.ValidateAsync(model, default)).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [VeloAutoData]
    public async Task ValidModel_ShouldLogin_LoginSucesss_ShouldReturnRediretToHome(LoginModel model, string url)
    {
        //  Arrange
        A.CallTo(() => Validator.ValidateAsync(model, default))
            .Returns(new ValidationResult());
        SignInManager.PasswordSignInAsyncResult = Microsoft.AspNetCore.Identity.SignInResult.Success;
        A.CallTo(() => Router.ToHome()).Returns(url);

        //  Act
        IActionResult result = await Sut.Login(model);

        //  Assert
        using (new AssertionScope())
        {
            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be(url);

        }
        A.CallTo(() => Validator.ValidateAsync(model, default)).MustHaveHappenedOnceExactly();
        A.CallTo(() => Router.ToHome()).MustHaveHappenedOnceExactly();
    }
}
