using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BraunauMobil.VeloBasar.ViewModels;

namespace BraunauMobil.VeloBasar.Pages.Basars
{
    [Authorize]
    public class ListModel : BasarPageModel, ISearchable
    {
        public ListModel(VeloBasarContext context) : base(context)
        {
        }

        public string CurrentFilter { get; set; }

        public PaginatedListViewModel<Basar> Basars { get;set; }

        public string MyPath => "/Basars/List";

        public async Task<IActionResult> OnGetAsync(string currentFilter, string searchString, int? pageIndex, int? basarId)
        {
            await LoadBasarAsync(basarId);

            CurrentFilter = searchString;
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            var brandIq = Context.Basar.GetMany(searchString);
            var pageSize = 10;
            Basars = await PaginatedListViewModel<Basar>.CreateAsync(Basar, brandIq.AsNoTracking(), pageIndex ?? 1, pageSize, Request.Path, GetRoute);

            return Page();
        }
        public IDictionary<string, string> GetDeleteRoute(Basar item)
        {
            return GetItemRoute("basarToDeleteId", item);
        }
        public IDictionary<string, string> GetEditRoute(Basar item)
        {
            return GetItemRoute("basarToEditId", item);
        }
        public IDictionary<string, string> GetSetStateRoute(Basar item, bool? stateToSet = null)
        {
            return GetItemRoute("basarToSetStateId", item, stateToSet);
        }
        public IDictionary<string, string> GetItemRoute(string itemIdKey, Basar item, bool? stateToSet = null)
        {
            var route = GetRoute();
            route.Add(itemIdKey, item.Id.ToString());
            route.Add("pageIndex", Basars.PageIndex.ToString());
            if (stateToSet != null)
            {
                route.Add("state", stateToSet.ToString());
            }
            return route;
        }
        public async Task<bool> CanDeleteAsync(Basar item)
        {
            return await Context.CanDeleteBasarAsync(item);
        }
    }
}
