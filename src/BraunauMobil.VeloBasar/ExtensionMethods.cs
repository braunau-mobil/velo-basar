using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace BraunauMobil.VeloBasar
{
    public static class ExtensionMethods
    {
        public static string AsJsArray<T>(this IEnumerable<T> values, Func<T, string> itemToString)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

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
        public static VeloPage GetPage<TPageModel>(this ISearchable searchable, int pageIndex, int? pageSize)
        {
            if (searchable == null) throw new ArgumentNullException(nameof(searchable));

            return new VeloPage
            {
                Page = RoutingHelper.GetPageForModel<TPageModel>(),
                Parameter = new SearchAndPaginationParameter
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    SearchString = searchable.SearchString
                }
            };
        }
        public static string GetPath(this LinkGenerator linkGenerator, VeloPage page)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            return linkGenerator.GetPathByPage(page.Page, values: page.Parameter);
        }
        public static IHtmlContent JsLiteral(this IHtmlHelper htmlHelper, IEnumerable<int> values)
        {
            if (htmlHelper == null) throw new ArgumentNullException(nameof(htmlHelper));
            return htmlHelper.Raw(values.AsJsArray(x => $"{x}"));
        }
        public static IHtmlContent JsLiteral(this IHtmlHelper htmlHelper, IEnumerable<string> values)
        {
            if (htmlHelper == null) throw new ArgumentNullException(nameof(htmlHelper));
            return htmlHelper.Raw(values.AsJsArray(x => $"\"{x}\""));
        }
        public static IHtmlContent JsCode(this IHtmlHelper htmlHelper, IEnumerable<string> values)
        {
            if (htmlHelper == null) throw new ArgumentNullException(nameof(htmlHelper));
            return htmlHelper.Raw(values.AsJsArray(x => x));
        }
        public static bool NextBool(this Random rand)
        {
            if (rand == null) throw new ArgumentNullException(nameof(rand));
            return rand.NextDouble() > 0.5;
        }
        public static double NextGaussian(this Random rand, double mean, double stdDev)
        {
            if (rand == null) throw new ArgumentNullException(nameof(rand));

            //  yehaw random stackoverflow code: https://stackoverflow.com/questions/218060/random-gaussian-variables
            var u1 = 1.0 - rand.NextDouble();
            var u2 = 1.0 - rand.NextDouble();
            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            return mean + stdDev * randStdNormal;
        }
        public static IActionResult RedirectToPage<TPageModel>(this PageModel pageModel, object parameter = null) where TPageModel : PageModel
        {
            if (pageModel == null) throw new ArgumentNullException(nameof(pageModel));
            return pageModel.RedirectToPage(RoutingHelper.GetPageForModel<TPageModel>(), parameter);
        }
        public static string Truncate(this string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            if (input.Length <= maxLength)
            {
                return input;
            }
            return input.Substring(0, maxLength);
        }
    }
}
