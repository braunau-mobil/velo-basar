using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Tests.Mockups;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
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

    public static TResult AssertDb<TResult>(this IServiceProvider services, Func<VeloDbContext, TResult> what)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(what);

        using IServiceScope scope = services.CreateScope();
        VeloDbContext db = scope.ServiceProvider.GetRequiredService<VeloDbContext>();
        using (new AssertionScope())
        {
            return what(db);
        }
    }

    public static ProductEntity AssertProduct(this VeloDbContext db, int productId)
    {
        ArgumentNullException.ThrowIfNull(db);

        return db.Products.AsNoTracking().Should().Contain(p => p.Id == productId).Subject;
    }

    public static void AssertProductStorageState(this VeloDbContext db, int productId, StorageState expectedStorageState)
    {
        ArgumentNullException.ThrowIfNull(db);

        ProductEntity product = db.AssertProduct(productId);
        product.StorageState.Should().Be(expectedStorageState);
    }

    public static void AssertProductValueState(this VeloDbContext db, int productId, ValueState expectedValueState)
    {
        ArgumentNullException.ThrowIfNull(db);

        ProductEntity product = db.AssertProduct(productId);
        product.ValueState.Should().Be(expectedValueState);
    }

    public static TransactionEntity AssertTransaction(this VeloDbContext db, int basarId, TransactionType type, int number)
    {
        ArgumentNullException.ThrowIfNull(db);

        return db.Transactions
            .Include(t => t.Seller)
            .Include(t => t.Products)
                .ThenInclude(pt => pt.Product)
            .AsNoTracking()
            .Should().Contain(t => t.BasarId == basarId && t.Type == type && t.Number == number).Subject;
    }

    public static void ShouldBeLikeInserted(this ProductEntity product, AcceptProductModel acceptModel)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentNullException.ThrowIfNull(acceptModel);

        product.Brand.Should().Be(acceptModel.Entity.Brand);
        product.Color.Should().Be(acceptModel.Entity.Color);
        product.FrameNumber.Should().Be(acceptModel.Entity.FrameNumber);
        product.Description.Should().Be(acceptModel.Entity.Description);
        product.Price.Should().Be(acceptModel.Entity.Price);
        product.TireSize.Should().Be(acceptModel.Entity.TireSize);
        product.TypeId.Should().Be(acceptModel.Entity.TypeId);
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

        CookiesMock cookies = scope.ServiceProvider.GetRequiredService<CookiesMock>();

        HttpContext httpContext = new DefaultHttpContext()
        {
            RequestServices = scope.ServiceProvider,
        };
        httpContext.Features.Set<IRequestCookiesFeature>(new RequestCookiesFeature(cookies));
        httpContext.Features.Set<IResponseCookiesFeature>(new ResponseCookiesFeatureWrapper(cookies));

        IHttpContextAccessor httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        httpContextAccessor.HttpContext = httpContext;

        IControllerFactory controllerFactory = scope.ServiceProvider.GetRequiredService<IControllerFactory>();
        object controller = controllerFactory.CreateController(new ControllerContext
        {
            ActionDescriptor = new ControllerActionDescriptor
            {
                ControllerTypeInfo = typeof(TController).GetTypeInfo()
            },
            HttpContext = httpContext
        });

        return (TController)controller;
    }
}
