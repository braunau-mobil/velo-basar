using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests;

public abstract class TestStepBase
{
    public TestStepBase(TestContext testContext)
    {
        Context = testContext;
    }

    protected TestContext Context { get; }

    public abstract Task Run();

    public void AssertDb(Action<VeloDbContext> what)
    {
        ArgumentNullException.ThrowIfNull(what);

        Context.Services.AssertDb(what);
    }

    public TResult AssertDb<TResult>(Func<VeloDbContext, TResult> what)
    {
        ArgumentNullException.ThrowIfNull(what);

        return Context.Services.AssertDb(what);
    }

    public async Task<ProductEntity> EnterProduct(int acceptSessionId, Action<AcceptProductModel> customize)
    {
        AcceptProductModel acceptProductModel = await Do<AcceptProductController, AcceptProductModel>(async controller =>
        {
            IActionResult result = await controller.Create(acceptSessionId);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().Be("CreateEdit");
            view.ViewData.ModelState.IsValid.Should().BeTrue();
            return view.Model.Should().BeOfType<AcceptProductModel>().Subject;
        });

        customize(acceptProductModel);

        await Do<AcceptProductController>(async controller =>
        {
            IActionResult result = await controller.Create(acceptProductModel.Entity);

            RedirectResult redirect = result.Should().BeOfType<RedirectResult>().Subject;
            redirect.Url.Should().Be($"//sessionId={acceptSessionId}&action=Create&controller=AcceptProduct");
        });

        return AssertDb(db =>
        {
            return db.Products.AsNoTracking().Should().Contain(p => p.SessionId == acceptSessionId && p.Price == acceptProductModel.Entity.Price).Subject;
        });
    }

    public void Do<TController>(Action<TController> what)
        where TController : Controller
    {
        ArgumentNullException.ThrowIfNull(what);

        Context.Services.Do(what);
    }

    public async Task Do<TController>(Func<TController, Task> what)
        where TController : Controller
    {
        ArgumentNullException.ThrowIfNull(what);

        await Context.Services.Do(what);
    }

    public TResult Do<TController, TResult>(Func<TController, TResult> what)
        where TController : Controller
    {
        ArgumentNullException.ThrowIfNull(what);

        return Context.Services.Do(what);
    }

    public async Task<TResult> Do<TController, TResult>(Func<TController, Task<TResult>> what)
        where TController : Controller
    {
        ArgumentNullException.ThrowIfNull(what);

        return await Context.Services.Do(what);
    }
}
