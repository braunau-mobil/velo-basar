using BraunauMobil.VeloBasar.Logic.Generic;
using BraunauMobil.VeloBasar.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Pages.Generic
{
    public class EditPageModel<TModel> : BasePageModel<TModel> where TModel : IModel, new()
    {
        private readonly ICrudContext<TModel> _context;

        public EditPageModel(ICrudContext<TModel> context)
        {
            _context = context;
        }

        [BindProperty]
        public TModel Item { get; set; }

        public async Task<IActionResult> OnGetAsync(BasePageParameter parameter)
        {
            Parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));

            Item = await _context.GetAsync(Parameter.Id);

            if (Item == null)
            {
                return NotFound();
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(BasePageParameter parameter)
        {
            Parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _context.UpdateAsync(Item);
            return RedirectToListOrigin();
        }
    }
}
