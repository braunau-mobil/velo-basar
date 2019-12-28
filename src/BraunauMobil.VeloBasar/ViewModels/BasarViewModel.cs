using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;

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
    }
}
