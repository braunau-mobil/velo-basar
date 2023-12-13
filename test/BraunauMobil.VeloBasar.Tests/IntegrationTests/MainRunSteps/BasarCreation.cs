﻿using Microsoft.AspNetCore.Mvc;
using Xan.AspNetCore.Mvc.Crud;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests.MainRunSteps;

public static class BasarCreation
{
    private const string _basarName = "1. Fahrradbasar";
    private const string _basarLocation = "Braunau am Inn";
    private const int _productCommissionPercentage = 10;
    private static readonly DateTime _basarDate = new (2063, 04, 05);

    public static async Task Run(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);

        //  Leave defaults
        BasarEntity basar = await services.Do<CrudController<BasarEntity>, BasarEntity>(async controller =>
        {
            IActionResult result = await controller.Create();

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().Be("CrudCreate");
            CrudModel<BasarEntity> crudModel = view.Model.Should().BeOfType<CrudModel<BasarEntity>>().Subject;
            crudModel.Entity.Should().NotBeNull();
            return crudModel.Entity;
        });

        await services.Do<CrudController<BasarEntity>>(async controller =>
        {
            IActionResult result = await controller.Create(basar);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().Be("CrudCreate");
            view.ViewData.ModelState.IsValid.Should().BeFalse();
            view.ViewData.ModelState.Should().ContainKey(nameof(BasarEntity.Name));
            CrudModel<BasarEntity> crudModel = view.Model.Should().BeOfType<CrudModel<BasarEntity>>().Subject;
            crudModel.Entity.Should().Be(basar);
        });

        services.AssertDb(db =>
        {
            db.Basars.Should().BeEmpty();
        });

        //  Name is empty
        basar.Name = "";
        basar.ProductCommissionPercentage = _productCommissionPercentage;
        basar.Date = _basarDate;
        basar.Location = _basarLocation;

        await services.Do<CrudController<BasarEntity>>(async controller =>
        {
            IActionResult result = await controller.Create(basar);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().Be("CrudCreate");
            view.ViewData.ModelState.IsValid.Should().BeFalse();
            view.ViewData.ModelState.Should().ContainKey(nameof(BasarEntity.Name));
            CrudModel<BasarEntity> crudModel = view.Model.Should().BeOfType<CrudModel<BasarEntity>>().Subject;
            crudModel.Entity.Should().Be(basar);
        });

        services.AssertDb(db =>
        {
            db.Basars.Should().BeEmpty();
        });

        //  ProductCommissionPercentage is out of range
        basar.Name = _basarName;
        basar.ProductCommissionPercentage = 1000;

        await services.Do<CrudController<BasarEntity>>(async controller =>
        {
            IActionResult result = await controller.Create(basar);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().Be("CrudCreate");
            view.ViewData.ModelState.IsValid.Should().BeFalse();
            view.ViewData.ModelState.Should().ContainKey(nameof(BasarEntity.ProductCommissionPercentage));
            CrudModel<BasarEntity> crudModel = view.Model.Should().BeOfType<CrudModel<BasarEntity>>().Subject;
            crudModel.Entity.Should().Be(basar);
        });

        services.AssertDb(db =>
        {
            db.Basars.Should().BeEmpty();
        });

        //  Valid basar
        basar.ProductCommissionPercentage = _productCommissionPercentage;

        await services.Do<CrudController<BasarEntity>>(async controller =>
        {
            IActionResult result = await controller.Create(basar);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//PageIndex=1&State=Enabled&action=List&controller=Basar");
        });

        //  Assert
        services.AssertDb(db =>
        {
            V.FirstBasar = db.Basars.Should().ContainSingle(basar => basar.Name == _basarName).Subject;
            V.FirstBasar.ProductCommissionPercentage.Should().Be(_productCommissionPercentage);
            V.FirstBasar.Date.Should().Be(_basarDate);
            V.FirstBasar.Location.Should().Be(_basarLocation);
        });
    }
}
