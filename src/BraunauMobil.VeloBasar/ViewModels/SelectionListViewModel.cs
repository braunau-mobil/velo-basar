using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class SelectionListViewModel<T> : ListViewModel<T>
    {
        private readonly string _targetIdParameterName;

        public SelectionListViewModel(Basar basar, IEnumerable<T> list, string targetPage, string targetIdParameterName) : base(basar, list)
        {
            TargetPage = targetPage;
            _targetIdParameterName = targetIdParameterName;
        }

        public string TargetPage { get; set; }

        public IDictionary<string, string> GetRoute(int id)
        {
            var route = GetRoute();
            route.Add(_targetIdParameterName, id.ToString());
            return route;
        }
    }
}
