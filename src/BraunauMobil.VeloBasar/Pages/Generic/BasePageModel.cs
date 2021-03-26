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
        public VeloPage ListPage(object parameter = null)
        {
            return new VeloPage { Page = ListPageRoute(), Parameter = parameter };
        }
        public string CreatePageRoute() => $"/{PageDirectory()}/Create";
        public string DeletePageRoute() => $"/{PageDirectory()}/Delete";
        public string EditPageRoute() => $"/{PageDirectory()}/Edit";
        public string ListPageRoute() => $"/{PageDirectory()}/List";
        public string SetStatePageRoute() => $"/{PageDirectory()}/SetState";
        private static string PageDirectory()
        {
            var type = typeof(TModel);
            return $"{type.Name}s";
        }

        public BasePageParameter Parameter { get; set; }

        public VeloPage ListPageOrigin()
        {
            var page = new VeloPage { Page = ListPageRoute() };
            if (Parameter != null)
            {
                page.Parameter = Parameter.OriginPageIndex;
            }
            return page;
        }
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
