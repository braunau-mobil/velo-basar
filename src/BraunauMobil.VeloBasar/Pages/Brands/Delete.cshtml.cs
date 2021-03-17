using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Pages.Generic;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Pages.Brands
{
    public class DeleteModel : DeleteModel<Brand, ListModel>
    {
        public DeleteModel(IBrandContext context) : base(context)
        {
        }
    }
}
