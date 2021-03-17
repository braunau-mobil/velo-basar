using BraunauMobil.VeloBasar.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Generic
{
    public class BasePageParameter
    {
        public int Id { get; set; }
        public int OriginPageIndex { get; set; }
    }
    public class BasePageModel<TModel> : PageModel where TModel : IModel, new()
    {
        public static VeloPage ListPage(object parameter = null)
        {
            return new VeloPage { Page = ListPageRoute(), Parameter = parameter };
        }
        public static string DeletePageRoute() => $"/{PageDirectory()}/Delete";
        public static string EditPageRoute() => $"/{PageDirectory()}/Edit";
        public static string ListPageRoute() => $"/{PageDirectory()}/List";
        public static string SetStatePageRoute() => $"/{PageDirectory()}/SetState";
        private static string PageDirectory()
        {
            var type = typeof(TModel);
            return $"{type.Name}s";
        }

        public BasePageParameter Parameter { get; set; }

        public VeloPage ListPageOrigin() => new VeloPage
        {
            Page = ListPageRoute(),
            Parameter = new SearchAndPaginationParameter
            {
                PageIndex = Parameter.OriginPageIndex
            }
        };
        public VeloPage ListPage(int pageIndex, int? pageSize) => new VeloPage
        {
            Page = ListPageRoute(),
            Parameter = new SearchAndPaginationParameter
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            }
        };

        public IActionResult RedirectToList() => RedirectToPage(ListPage());
        public IActionResult RedirectToListOrigin() => RedirectToPage(ListPageOrigin());
        
    }
}
