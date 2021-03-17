using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Authorization;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Pages.Generic;

namespace BraunauMobil.VeloBasar.Pages.Brands
{
    [Authorize]
    public class ListModel : ListPageModel<Brand, DeleteModel, EditModel, ListModel, SetStateModel>
    {
        public ListModel(IVeloContext context, IBrandContext brandContext) : base(context, brandContext)
        {
        }
    }
}
