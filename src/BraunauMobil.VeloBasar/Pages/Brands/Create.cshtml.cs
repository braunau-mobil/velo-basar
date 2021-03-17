using BraunauMobil.VeloBasar.Logic.Generic;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Pages.Generic;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.AspNetCore.Mvc.Localization;

namespace BraunauMobil.VeloBasar.Pages.Brands
{
    public class CreateModel : CreateModel<Brand, ListModel>
    {
        public CreateModel(ICrudContext<Brand> context, IHtmlLocalizer<SharedResource> localizer) : base(context, localizer)
        {
            Title = "Marke hinzufügen";
        }
    }
}