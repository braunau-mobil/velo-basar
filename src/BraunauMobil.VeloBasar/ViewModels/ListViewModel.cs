using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class ListViewModel<T> : BasarViewModel
    {
        public ListViewModel() { }
        public ListViewModel(Basar basar, IEnumerable<T> list) : base(basar)
        {
            List = new List<T>(list);
        }

        [BindProperty]
        public List<T> List
        {
            get;
            set;
        }
    }
}
