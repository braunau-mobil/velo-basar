using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class ListViewModel<T> : BasarViewModel
    {
        public ListViewModel(Basar basar, IEnumerable<T> list) : base(basar)
        {
            List = list;
        }

        public IEnumerable<T> List
        {
            get;
            set;
        }
    }
}
