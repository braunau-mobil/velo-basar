using Microsoft.AspNetCore.Mvc.Filters;

namespace BraunauMobil.VeloBasar.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class SkipVeloFiltersAttribute
    : Attribute
    , IResourceFilter
{
    public void OnResourceExecuted(ResourceExecutedContext context)
    { }

    public void OnResourceExecuting(ResourceExecutingContext context)
    { }
}
