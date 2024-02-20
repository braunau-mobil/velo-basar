using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text;
using Xan.Extensions.Collections.Generic;

namespace BraunauMobil.VeloBasar.Tests.Mockups;

public class LinkGeneratorMock
    : LinkGenerator
{
    private readonly Dictionary<string, RouteValueDictionary> _routeValues = [];

    public int Id(string url)
        => NamedIntParameter(url, "id");

    public int SessionId(string url)
        => NamedIntParameter(url, "sessionId");

    public int NamedIntParameter(string url, string parameterName)
        => _routeValues.Should().ContainKey(url)
            .WhoseValue.Should().ContainKey(parameterName)
            .WhoseValue.Should().BeOfType<int>().Subject;

    public override string? GetPathByAddress<TAddress>(HttpContext httpContext, TAddress address, RouteValueDictionary values, RouteValueDictionary? ambientValues = null, PathString? pathBase = null, FragmentString fragment = default, LinkOptions? options = null)
        => GetMockedUri(values, pathBase, fragment);

    public override string? GetPathByAddress<TAddress>(TAddress address, RouteValueDictionary values, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
        => GetMockedUri(values, pathBase, fragment);

    public override string? GetUriByAddress<TAddress>(HttpContext httpContext, TAddress address, RouteValueDictionary values, RouteValueDictionary? ambientValues = null, string? scheme = null, HostString? host = null, PathString? pathBase = null, FragmentString fragment = default, LinkOptions? options = null)
        => GetMockedUri(values, pathBase, fragment);

    public override string? GetUriByAddress<TAddress>(TAddress address, RouteValueDictionary values, string? scheme, HostString host, PathString pathBase = default, FragmentString fragment = default, LinkOptions? options = null)
        => GetMockedUri(values, pathBase, fragment);

    private string GetMockedUri(RouteValueDictionary values, PathString? pathBase = null, FragmentString fragment = default)
    {
        string url = $"{pathBase}/{fragment}/{ToString(values)}";
        _routeValues.Set(url, values);
        return url;
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
