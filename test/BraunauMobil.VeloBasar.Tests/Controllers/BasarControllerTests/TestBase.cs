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
        A.CallTo(() => Router.Basar).Returns(BasarRouter);
        
        Sut = new BasarController(BasarService, BasarRouter, ModelFactory, Validator);
    }

    protected IBasarRouter BasarRouter { get; } = X.StrictFake<IBasarRouter>();

    protected IBasarService BasarService { get; } = X.StrictFake<IBasarService> ();

    protected VeloFixture Fixture { get; } = new ();

    protected ICrudModelFactory<BasarEntity, ListParameter> ModelFactory { get; } = X.StrictFake<ICrudModelFactory<BasarEntity, ListParameter>>();

    protected IVeloRouter Router { get; } = X.StrictFake<IVeloRouter>();

    protected BasarController Sut { get; }

    protected IValidator<BasarEntity> Validator { get; } = X.StrictFake<IValidator<BasarEntity>>();
}
