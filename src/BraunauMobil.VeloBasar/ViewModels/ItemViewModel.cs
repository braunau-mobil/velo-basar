using BraunauMobil.VeloBasar.Pages.Generic;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class ItemViewModel<T> : IListItem
    {
        public string Alert { get; set; }
        public bool HasAlert { get; set; }
        [BindProperty]
        public T Item { get; set; }
        object IListItem.Item => Item;
        [BindProperty]
        public bool IsSelected { get; set; }
    }
}
    