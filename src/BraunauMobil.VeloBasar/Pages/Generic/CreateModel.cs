using BraunauMobil.VeloBasar.Logic.Generic;
using BraunauMobil.VeloBasar.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Pages.Generic
{
    public abstract class CreateModel<TModel, TListPageModel> : PageModel where TModel : IModel, new() where TListPageModel : PageModel
    {
        private readonly ICrudContext<TModel> _context;

        public CreateModel(ICrudContext<TModel> context)
        {
            _context = context;
        }

        [BindProperty]
        public TModel Item { get; set; }
        public string Title { get; set; }

        public IActionResult OnGet()
        {
            Item = new TModel();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _context.CreateAsync(Item);
            return this.RedirectToPage<TListPageModel>();
        }
    }
}
