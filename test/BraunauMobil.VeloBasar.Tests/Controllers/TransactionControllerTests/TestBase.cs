﻿using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Routing;
using BraunauMobil.VeloBasar.Tests.Mockups;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Controllers.TransactionControllerTests;

public class TestBase
{
    public TestBase()
    {
        A.CallTo(() => Router.Cancel).Returns(CancelRouter);

        Sut = new (TransactionService, Router, SignInManager, new TransactionSuccessModelValidator(Localizer));        
    }

    protected ICancelRouter CancelRouter { get; } = X.StrictFake<ICancelRouter>();

    protected Fixture Fixture { get; } = new ();

    protected IStringLocalizer<SharedResources> Localizer { get; } = Helpers.CreateActualLocalizer();

    protected IVeloRouter Router { get; } = X.StrictFake<IVeloRouter>();

    protected SignInManagerMock SignInManager { get; } = new ();

    protected TransactionController Sut { get; }

    protected ITransactionService TransactionService { get; } = X.StrictFake<ITransactionService>();
}
