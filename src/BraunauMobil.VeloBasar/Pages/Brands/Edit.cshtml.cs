using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Pages.Generic;

namespace BraunauMobil.VeloBasar.Pages.Brands
{
    public class EditModel : EditModel<Brand, ListModel>
    {
        public EditModel(IBrandContext context) : base(context)
        {
            Title = "Marke bearbeiten";
        }
    }
}
