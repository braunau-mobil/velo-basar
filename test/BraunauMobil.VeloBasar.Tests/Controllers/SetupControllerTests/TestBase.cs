﻿using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Routing;
using Microsoft.Extensions.Localization;

namespace BraunauMobil.VeloBasar.Tests.Controllers.SetupControllerTests;

public class TestBase
{
    public TestBase()
    {
        Sut = new (SetupService, Router, new InitializationConfigurationValidator(Localizer));
    }

    protected VeloFixture Fixture { get; } = new ();

    protected IStringLocalizer<SharedResources> Localizer { get; } = X.StringLocalizer;

    protected IVeloRouter Router { get; } = X.StrictFake<IVeloRouter>();

    protected SetupController Sut { get; }

    protected ISetupService SetupService { get; } = X.StrictFake<ISetupService>();
}
