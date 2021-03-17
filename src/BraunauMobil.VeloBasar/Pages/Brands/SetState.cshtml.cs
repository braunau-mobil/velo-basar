using BraunauMobil.VeloBasar.Logic.Generic;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Pages.Generic;

namespace BraunauMobil.VeloBasar.Pages.Brands
{
    public class SetStateModel : SetStatePageModel<Brand, ListModel>
    {
        public SetStateModel(ICrudContext<Brand> context) : base(context)
        {
        }
    }
}
