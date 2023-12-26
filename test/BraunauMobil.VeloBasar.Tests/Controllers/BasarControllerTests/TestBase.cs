using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Xan.AspNetCore.Mvc.Crud;
using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Tests.Controllers.BasarControllerTests;

public class TestBase
{
    public TestBase()
    {
        Sut = new BasarController(BasarService.Object, BasarRouter.Object, ModelFactory.Object, Validator.Object);

        Router.Setup(_ => _.Basar)
            .Returns(BasarRouter.Object);
    }

    public void VerifyNoOtherCalls()
    {
        BasarService.VerifyNoOtherCalls();
        BasarRouter.VerifyNoOtherCalls();
        ModelFactory.VerifyNoOtherCalls();
        Validator.VerifyNoOtherCalls();
    }

    protected Mock<IBasarRouter> BasarRouter { get; } = new();

    protected Mock<IBasarService> BasarService { get; } = new ();

    protected Fixture Fixture { get; } = new ();

    protected Mock<ICrudModelFactory<BasarEntity, ListParameter>> ModelFactory { get; } = new();

    protected Mock<IVeloRouter> Router { get; } = new();

    protected BasarController Sut { get; }

    protected Mock<IValidator<BasarEntity>> Validator { get; } = new();
}
