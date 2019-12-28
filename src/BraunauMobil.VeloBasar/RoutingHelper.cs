using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BraunauMobil.VeloBasar
{
    public static class RoutingHelper
    {
        [SuppressMessage("Globalization", "CA1307:Specify StringComparison")]
        public static string GetPageForModel<TPageModel>()
        {
            var type = typeof(TPageModel);
            var path = type.Namespace.Replace("BraunauMobil.VeloBasar.Pages", "").TrimStart('.').Replace('.', '/');
            var page = type.Name.Replace("Model", "");
            
            if (string.IsNullOrEmpty(path))
            {
                return $"/{page}";
            }
            
            return $"/{path}/{page}";
        }
        public static IDictionary<string, string> ConvertToRoute(object value)
        {
            var routeValueDictionary = new RouteValueDictionary(value);
            var route = new Dictionary<string, string>();
            foreach (var item in routeValueDictionary)
            {
                if (item.Value == null)
                {
                    continue;
                }
                route.Add(item.Key, item.Value.ToString());
            }
            return route;
        }
    }
}
