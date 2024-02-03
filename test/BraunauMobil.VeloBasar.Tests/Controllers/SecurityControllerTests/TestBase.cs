using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SecurityControllerTests;

public class TestBase
{
    public TestBase()
    {
        Sut = new(SignInManager, Router, A.Fake<ILogger<SecurityController>>(), Validator)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = RequestServices
                }
            }
        };

        A.CallTo(() => RequestServices.GetService(typeof(ITempDataDictionaryFactory)))
            .Returns(TempDataDictionaryFactory);
    }

    protected IServiceProvider RequestServices { get; } = X.StrictFake<IServiceProvider>();
    
    protected IVeloRouter Router { get; } = X.StrictFake<IVeloRouter>();

    protected SignInManagerMock SignInManager { get; } = new ();

    protected SecurityController Sut { get; }

    protected ITempDataDictionaryFactory TempDataDictionaryFactory = A.Fake<ITempDataDictionaryFactory>();

    protected IValidator<LoginModel> Validator { get; } = X.StrictFake<IValidator<LoginModel>>();
}
