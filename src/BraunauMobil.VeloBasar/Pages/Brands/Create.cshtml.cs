using BraunauMobil.VeloBasar.Logic.Generic;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Pages.Generic;

namespace BraunauMobil.VeloBasar.Pages.Brands
{
    public class CreateModel : CreateModel<Brand, ListModel>
    {
        public CreateModel(ICrudContext<Brand> context) : base(context)
        {
            Title = "Marke hinzufügen";
        }
    }
}