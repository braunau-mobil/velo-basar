using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class ItemViewModel<T>
    {
        public string Alert { get; set; }
        public bool HasAlert { get; set; }
        [BindProperty]
        public T Item { get; set; }
        [BindProperty]
        public bool IsSelected { get; set; }
    }
}
    