using BraunauMobil.VeloBasar.Logic.Generic;
using BraunauMobil.VeloBasar.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Pages.Generic
{
    public class DeletePageModel<TModel> : BasePageModel<TModel> where TModel : IModel, new()
    {
        private readonly ICrudContext<TModel> _context;

        public DeletePageModel(ICrudContext<TModel> context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(BasePageParameter parameter)
        {
            Parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));

            if (await _context.ExistsAsync(Parameter.Id))
            {
                await _context.DeleteAsync(Parameter.Id);
            }
            else
            {
                return NotFound();
            }
            return RedirectToListOrigin();
        }
    }
}
