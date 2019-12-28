using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace BraunauMobil.VeloBasar
{
    public static class ExtensionMethods
    {
        public static string AsJsArray<T>(this IEnumerable<T> values, Func<T, string> itemToString)
        {
            Contract.Requires(values != null);

            var sb = new StringBuilder();
            sb.Append("[");
            foreach (var val in values)
            {
                if (sb.Length != 1)
                {
                    sb.Append(',');
                }
                sb.Append(itemToString(val));
            }
            sb.Append("]");
            return sb.ToString();
        }
        public static VeloPage GetPage<TPageModel>(this PageModel page, object parameter = null)
        {
            return new VeloPage { Page = RoutingHelper.GetPageForModel<TPageModel>(), Parameter = parameter };
        }
        public static VeloPage GetPage<TPageModel>(this IRazorPage page, object parameter = null)
        {
            return new VeloPage { Page = RoutingHelper.GetPageForModel<TPageModel>(), Parameter = parameter };
        }
        public static string GetPath(this LinkGenerator linkGenerator, VeloPage page)
        {
            Contract.Requires(page != null);
            return linkGenerator.GetPathByPage(page.Page, values: page.Parameter);
        }
        public static IHtmlContent JsLiteral(this IHtmlHelper htmlHelper, IEnumerable<int> values)
        {
            Contract.Requires(htmlHelper != null);
            return htmlHelper.Raw(values.AsJsArray(x => $"{x}"));
        }
        public static IHtmlContent JsLiteral(this IHtmlHelper htmlHelper, IEnumerable<string> values)
        {
            Contract.Requires(htmlHelper != null);
            return htmlHelper.Raw(values.AsJsArray(x => $"\"{x}\""));
        }
        public static IHtmlContent JsCode(this IHtmlHelper htmlHelper, IEnumerable<string> values)
        {
            Contract.Requires(htmlHelper != null);
            return htmlHelper.Raw(values.AsJsArray(x => x));
        }
        public static double NextGaussian(this Random rand, double mean, double stdDev)
        {
            Contract.Requires(rand != null);

            //  yehaw random stackoverflow code: https://stackoverflow.com/questions/218060/random-gaussian-variables
            var u1 = 1.0 - rand.NextDouble();
            var u2 = 1.0 - rand.NextDouble();
            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            return mean + stdDev * randStdNormal;
        }
        public static IActionResult RedirectToPage<TPageModel>(this PageModel pageModel, object parameter = null) where TPageModel : PageModel
        {
            Contract.Requires(pageModel != null);
            return pageModel.RedirectToPage(RoutingHelper.GetPageForModel<TPageModel>(), parameter);
        }

    }
}
