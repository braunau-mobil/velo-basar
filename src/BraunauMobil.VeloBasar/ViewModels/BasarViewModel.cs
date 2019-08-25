using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class BasarViewModel
    {
        public BasarViewModel(Basar basar)
        {
            Basar = basar;
        }

        [BindProperty]
        public Basar Basar { get; set; }

        public virtual IDictionary<string, string> GetRoute()
        {
            var route = new Dictionary<string, string>();
            if (Basar != null)
            {
                route.Add("basarId", Basar.Id.ToString());
            }
            return route;
        }
    }
}
