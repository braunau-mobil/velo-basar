using Microsoft.AspNetCore.Mvc;
using Xan.AspNetCore.Mvc.Crud;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests.MainRunSteps;

public static class BasarCreation
{
    public static async Task Run(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(nameof(services));

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
        basar.ProductCommissionPercentage = V.FirstBasar.ProductCommissionPercentage;
        basar.Date = V.FirstBasar.Date;
        basar.Location = V.FirstBasar.Location;

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
        basar.Name = V.FirstBasar.Name;
        basar.ProductCommissionPercentage = V.FirstBasar.ProductCommissionPercentage * 100;
        basar.Date = V.FirstBasar.Date;
        basar.Location = V.FirstBasar.Location;

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
        basar.Name = V.FirstBasar.Name;
        basar.ProductCommissionPercentage = V.FirstBasar.ProductCommissionPercentage;
        basar.Date = V.FirstBasar.Date;
        basar.Location = V.FirstBasar.Location;

        await services.Do<CrudController<BasarEntity>>(async controller =>
        {
            IActionResult result = await controller.Create(basar);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//PageIndex=1&State=Enabled&action=List&controller=Basar");
        });

        //  Assert
        services.AssertDb(db =>
        {
            BasarEntity actualBasar = db.Basars.Should().ContainSingle(basar => basar.Name == V.FirstBasar.Name).Subject;
            actualBasar.ProductCommission.Should().Be(V.FirstBasar.ProductCommission);
            actualBasar.Date.Should().Be(V.FirstBasar.Date);
            actualBasar.Location.Should().Be(V.FirstBasar.Location);
        });
    }
}
