﻿using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar
{
    public class BasarPageModel : PageModel
    {
        public BasarPageModel(VeloBasarContext context)
        {
            Context = context;
        }

        protected VeloBasarContext Context { get; }

        [BindProperty]
        public Basar Basar { get; set; }

        public string BackToPage { get; set; } = "./List";

        public IDictionary<string, string> BackToRoute { get; set; }

        protected virtual void DecorateRoute(IDictionary<string, string> route)
        {
        }

        protected string GetReferer()
        {
            return Request.Headers["Referer"];
        }

        protected async Task LoadBasarAsync(int? basarId)
        {
#if DEBUG
            basarId = 1;
#endif
            if (basarId != null)
            {
                Basar = await Context.GetBasarAsync(basarId.Value);
            }
        }

        public virtual IDictionary<string, string> GetRoute()
        {
            var route = new Dictionary<string, string>();
            if (Basar != null)
            {
                route.Add("basarId", Basar.Id.ToString());
            }
            DecorateRoute(route);
            return route;
        }
    }
}
