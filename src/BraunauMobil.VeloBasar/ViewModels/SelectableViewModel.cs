using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.ViewModels
{
    public class SelectableViewModel<T>
    {
        [BindProperty]
        public T Value { get; set; }
        [BindProperty]
        public bool IsSelected { get; set; }
    }
}
