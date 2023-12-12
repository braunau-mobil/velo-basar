using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests;

public static class Extensions
{
    public static TController CreateController<TController>(this IServiceScope scope)
        where TController : Controller
    {
        ArgumentNullException.ThrowIfNull(scope);

        TController controller = scope.ServiceProvider.GetRequiredService<TController>();

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
            {
                RequestServices = scope.ServiceProvider
            }
        };

        return controller;
    }
}
