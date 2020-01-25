using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace BraunauMobil.VeloBasar
{
    public class SearchAndPaginationParameter
    {
        public string SearchString { get; set; }
        [SuppressMessage("Naming", "CA1721:Property names should not match get methods")]
        public int? PageIndex { get; set; }
        [SuppressMessage("Naming", "CA1721:Property names should not match get methods")]
        public int? PageSize { get; set; }

        public int GetPageIndex()
        {
            return PageIndex ?? 1;
        }
        public int GetPageSize(PageModel pageModel)
        {
            Contract.Requires(pageModel != null);

            var modelName = pageModel.GetType().FullName;

            var pageSize = PageSize;
            if (pageSize == null)
            {
                pageSize = pageModel.Request.Cookies.GetPageSize(modelName);
            }
            pageModel.Response.Cookies.SetPageSize(modelName, pageSize.Value);

            return pageSize.Value;
        }
    }
}
