using BraunauMobil.VeloBasar.Logic.Generic;
using BraunauMobil.VeloBasar.Models.Interfaces;
using BraunauMobil.VeloBasar.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Pages.Generic
{
    public abstract class CreateModel<TModel, TListPageModel> : PageModel where TModel : IModel, new() where TListPageModel : PageModel
    {
        private readonly ICrudContext<TModel> _context;
        private readonly IHtmlLocalizer<SharedResource> _localizer;

        public CreateModel(ICrudContext<TModel> context, IHtmlLocalizer<SharedResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        [BindProperty]
        public TModel Item { get; set; }
        public string Title { get; set; }

        public IActionResult OnGet()
        {
            ViewData["Title"] = _localizer[Title];
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
