using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class ListCommand<T>
    {
        private readonly Func<T, bool> _isEnabled;
        private readonly Func<T, IDictionary<string, string>> _getRoute;

        public ListCommand() : this(null, null)
        {
        }
        public ListCommand(Func<T, bool> isEnabled) : this(isEnabled, null)
        {
        }
        public ListCommand(Func<T, IDictionary<string, string>> getRoute) : this(null, getRoute)
        {
        }
        public ListCommand(Func<T, bool> isEnabled, Func<T, IDictionary<string, string>> getRoute)
        {
            _isEnabled = isEnabled ?? (x => true);
            _getRoute = getRoute ?? (x => new Dictionary<string, string>());
        }

        public string Text { get; set; }
        public string Page { get; set; }

        public bool IsEnabled(T item) => _isEnabled(item);
        public IDictionary<string, string> GetRoute(T item) => _getRoute(item);
    }

    public class ListViewModel<T> : BasarViewModel
    {
        public ListViewModel() : this(null, null) { }
        public ListViewModel(Basar basar, IList<T> items, ListCommand<T>[] commands = null) : base(basar)
        {
            List = items;
            Commands = commands ?? new ListCommand<T>[0];
        }

        public ListCommand<T>[] Commands { get; set; }
        [BindProperty]
        public IList<T> List { get; set; }
    }
}
