using BraunauMobil.VeloBasar.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests;

public static class Extensions
{
    public static void AssertDb(this IServiceProvider services, Action<VeloDbContext> what)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(what);

        using IServiceScope scope = services.CreateScope();
        VeloDbContext db = scope.ServiceProvider.GetRequiredService<VeloDbContext>();
        using (new AssertionScope())
        {
            what(db);
        }
    }

    public static void Do<TController>(this IServiceProvider services,  Action<TController> what)
        where TController : Controller
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(what);

        using IServiceScope scope = services.CreateScope();
        TController controller = scope.CreateController<TController>();

        what(controller);
    }

    public static async Task Do<TController>(this IServiceProvider services, Func<TController, Task> what)
        where TController : Controller
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(what);

        using IServiceScope scope = services.CreateScope();
        TController controller = scope.CreateController<TController>();

        await what(controller);
    }

    public static TResult Do<TController, TResult>(this IServiceProvider services, Func<TController, TResult> what)
        where TController : Controller
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(what);

        using IServiceScope scope = services.CreateScope();
        TController controller = scope.CreateController<TController>();

        return what(controller);
    }

    public static async Task<TResult> Do<TController, TResult>(this IServiceProvider services, Func<TController, Task<TResult>> what)
        where TController : Controller
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(what);

        using IServiceScope scope = services.CreateScope();
        TController controller = scope.CreateController<TController>();

        return await what(controller);
    }

    public static TController CreateController<TController>(this IServiceScope scope)
        where TController : Controller
    {
        ArgumentNullException.ThrowIfNull(scope);

        IControllerFactory controllerFactory = scope.ServiceProvider.GetRequiredService<IControllerFactory>();
        object controller = controllerFactory.CreateController(new ControllerContext
        {
            ActionDescriptor = new ControllerActionDescriptor
            {
                ControllerTypeInfo = typeof(TController).GetTypeInfo()
            },
            HttpContext = new DefaultHttpContext()
            {
                RequestServices = scope.ServiceProvider
            }
        });

        return (TController)controller;
    }
}
