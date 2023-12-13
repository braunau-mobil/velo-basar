using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Xan.AspNetCore.Mvc.Crud;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests.BasarTests;

public class Create
    : TestBase
{
    [Fact]
    public async Task ValidBasar()
    {
        //  Arrance
        await V.Init(Services);

        //  Act
        BasarEntity basar = await Do<CrudController<BasarEntity>, BasarEntity>(async controller =>
        {
            IActionResult result = await controller.Create();

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().Be("CrudCreate");
            CrudModel<BasarEntity> crudModel = view.Model.Should().BeOfType<CrudModel<BasarEntity>>().Subject;
            crudModel.Entity.Should().NotBeNull();
            return crudModel.Entity;
        });

        basar.Name = V.FirstBasar.Name;
        basar.ProductCommissionPercentage = V.FirstBasar.ProductCommissionPercentage;
        basar.Date = V.FirstBasar.Date;
        basar.Location = V.FirstBasar.Location;

        await Do<CrudController<BasarEntity>>(async controller =>
        {
            IActionResult result = await controller.Create(basar);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be("//PageIndex=1&State=Enabled&action=List&controller=Basar");
        });

        //  Assert
        using VeloDbContext db = Services.GetRequiredService<VeloDbContext>();
        using (new AssertionScope())
        {
            BasarEntity actualBasar = db.Basars.Should().ContainSingle(basar => basar.Name == V.FirstBasar.Name).Subject;
            actualBasar.ProductCommission.Should().Be(V.FirstBasar.ProductCommission);
            actualBasar.Date.Should().Be(V.FirstBasar.Date);
            actualBasar.Location.Should().Be(V.FirstBasar.Location);
        }
    }

    [Fact]
    public async Task LeaveDefaults()
    {
        //  Arrance
        await V.Init(Services);

        //  Act
        BasarEntity basar = await Do<CrudController<BasarEntity>, BasarEntity>(async controller =>
        {
            IActionResult result = await controller.Create();

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().Be("CrudCreate");
            CrudModel<BasarEntity> crudModel = view.Model.Should().BeOfType<CrudModel<BasarEntity>>().Subject;
            crudModel.Entity.Should().NotBeNull();
            return crudModel.Entity;
        });

        await Do<CrudController<BasarEntity>>(async controller =>
        {
            IActionResult result = await controller.Create(basar);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().Be("CrudCreate");
            view.ViewData.ModelState.IsValid.Should().BeFalse();
            view.ViewData.ModelState.Should().ContainKey(nameof(BasarEntity.Name));
            CrudModel<BasarEntity> crudModel = view.Model.Should().BeOfType<CrudModel<BasarEntity>>().Subject;
            crudModel.Entity.Should().Be(basar);
        });

        //  Assert
        using VeloDbContext db = Services.GetRequiredService<VeloDbContext>();
        using (new AssertionScope())
        {
            db.Basars.Should().BeEmpty();
        }
    }

    [Fact]
    public async Task NameIsNull()
    {
        //  Arrance
        await V.Init(Services);

        //  Act
        BasarEntity basar = await Do<CrudController<BasarEntity>, BasarEntity>(async controller =>
        {
            IActionResult result = await controller.Create();

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().Be("CrudCreate");
            CrudModel<BasarEntity> crudModel = view.Model.Should().BeOfType<CrudModel<BasarEntity>>().Subject;
            crudModel.Entity.Should().NotBeNull();
            return crudModel.Entity;
        });

        basar.Name = "";
        basar.ProductCommissionPercentage = V.FirstBasar.ProductCommissionPercentage;
        basar.Date = V.FirstBasar.Date;
        basar.Location = V.FirstBasar.Location;

        await Do<CrudController<BasarEntity>>(async controller =>
        {
            IActionResult result = await controller.Create(basar);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().Be("CrudCreate");
            view.ViewData.ModelState.IsValid.Should().BeFalse();
            view.ViewData.ModelState.Should().ContainKey(nameof(BasarEntity.Name));
            CrudModel<BasarEntity> crudModel = view.Model.Should().BeOfType<CrudModel<BasarEntity>>().Subject;
            crudModel.Entity.Should().Be(basar);
        });

        //  Assert
        using VeloDbContext db = Services.GetRequiredService<VeloDbContext>();
        using (new AssertionScope())
        {
            db.Basars.Should().BeEmpty();
        }
    }

    [Fact]
    public async Task ProductCommissionIsOutOfRange()
    {
        //  Arrance
        await V.Init(Services);

        //  Act
        BasarEntity basar = await Do<CrudController<BasarEntity>, BasarEntity>(async controller =>
        {
            IActionResult result = await controller.Create();

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().Be("CrudCreate");
            CrudModel<BasarEntity> crudModel = view.Model.Should().BeOfType<CrudModel<BasarEntity>>().Subject;
            crudModel.Entity.Should().NotBeNull();
            return crudModel.Entity;
        });

        basar.Name = V.FirstBasar.Name;
        basar.ProductCommissionPercentage = V.FirstBasar.ProductCommissionPercentage * 100;
        basar.Date = V.FirstBasar.Date;
        basar.Location = V.FirstBasar.Location;

        await Do<CrudController<BasarEntity>>(async controller =>
        {
            IActionResult result = await controller.Create(basar);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().Be("CrudCreate");
            view.ViewData.ModelState.IsValid.Should().BeFalse();
            view.ViewData.ModelState.Should().ContainKey(nameof(BasarEntity.ProductCommissionPercentage));
            CrudModel<BasarEntity> crudModel = view.Model.Should().BeOfType<CrudModel<BasarEntity>>().Subject;
            crudModel.Entity.Should().Be(basar);
        });

        //  Assert
        using VeloDbContext db = Services.GetRequiredService<VeloDbContext>();
        using (new AssertionScope())
        {
            db.Basars.Should().BeEmpty();
        }
    }
}
