using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text;

namespace BraunauMobil.VeloBasar.Tests.Mockups;

public class LinkGeneratorMock
    : LinkGenerator
{
    public override string? GetPathByAddress<TAddress>(HttpContext httpContext, TAddress address, RouteValueDictionary values, RouteValueDictionary? ambientValues = null, PathString? pathBase = null, FragmentString fragment = default, LinkOptions? options = null)
    {
        return $"{pathBase}/{fragment}/{ToString(values)}";
    }

    public override string? GetPathByAddress<TAddress>(TAddress address, RouteValueDictionary values, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
    {
        return $"{pathBase}/{fragment}/{ToString(values)}";
    }

    public override string? GetUriByAddress<TAddress>(HttpContext httpContext, TAddress address, RouteValueDictionary values, RouteValueDictionary? ambientValues = null, string? scheme = null, HostString? host = null, PathString? pathBase = null, FragmentString fragment = default, LinkOptions? options = null)
    {
        return $"{pathBase}/{fragment}/{ToString(values)}";
    }

    public override string? GetUriByAddress<TAddress>(TAddress address, RouteValueDictionary values, string? scheme, HostString host, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
    {
        return $"{pathBase}/{fragment}/{ToString(values)}";
    }

    private static string ToString(RouteValueDictionary values)
    {
        StringBuilder sb = new();
        foreach (KeyValuePair<string, object?> pair in values)
        {
            if (pair.Value is null)
            {
                continue;
            }

            if (sb.Length > 0)
            {
                sb.Append('&');
            }

            sb.Append($"{pair.Key}={pair.Value}");
        }
        return sb.ToString();
    }
}
