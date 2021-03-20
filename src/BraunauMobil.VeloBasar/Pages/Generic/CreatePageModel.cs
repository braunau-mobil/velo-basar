using BraunauMobil.VeloBasar.Logic.Generic;
using BraunauMobil.VeloBasar.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Pages.Generic
{
    public class CreatePageModel<TModel> : BasePageModel<TModel>, ICreatePageModel where TModel : IModel, new()
    {
        private readonly ICrudContext<TModel> _context;

        public CreatePageModel(ICrudContext<TModel> context)
        {
            _context = context;
        }

        [BindProperty]
        public TModel Item { get; set; }
        object ICreatePageModel.Item { get => Item; }

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
            return RedirectToList();
        }
    }
}
