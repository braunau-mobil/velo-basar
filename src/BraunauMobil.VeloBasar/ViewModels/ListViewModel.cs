using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class ListCommand<T>
    {
        private readonly Func<T, bool> _isEnabled;
        private readonly Func<T, VeloPage> _getPage;

        public ListCommand() : this(null, null)
        {
        }
        public ListCommand(Func<T, bool> isEnabled) : this(isEnabled, null)
        {
        }
        public ListCommand(Func<T, VeloPage> getPage) : this(null, getPage)
        {
        }
        public ListCommand(Func<T, bool> isEnabled, Func<T, VeloPage> getPage)
        {
            _isEnabled = isEnabled ?? (x => true);
            _getPage = getPage;
        }

        public string Text { get; set; }

        public bool IsEnabled(T item) => _isEnabled(item);
        public VeloPage GetPage(T item) => _getPage(item);
    }

    public class ListViewModel<T> : BasarViewModel
    {
        public ListViewModel() : base(null)
        {
            Commands = Array.Empty<ListCommand<T>>();
        }
        public ListViewModel(Basar basar, IReadOnlyList<T> items, ListCommand<T>[] commands = null) : this(basar, items.Select(i => new ItemViewModel<T> { Item = i }).ToList() , commands)
        {
        }
        public ListViewModel(Basar basar, IReadOnlyList<ItemViewModel<T>> items, ListCommand<T>[] commands = null) : base(basar)
        {
            List = items;
            Commands = commands ?? Array.Empty<ListCommand<T>>();
        }

        public ListCommand<T>[] Commands { get; set; }
        [BindProperty]
        public IReadOnlyList<ItemViewModel<T>> List { get; set; }
    }
}
