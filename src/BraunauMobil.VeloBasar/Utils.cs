using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar
{
    public static class Utils
    {
        public static string GetPageForModel<TPageModel>()
        {
            //  @todo evil hack from hell!!
            var type = typeof(TPageModel);
            var page = type.Namespace;
            page = page.Replace("BraunauMobil.VeloBasar.Pages.", "");
            page = page.Replace(".", "/");
            page += "/";
            page += type.Name.Replace("Model", "");

            return "/" + page;
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
