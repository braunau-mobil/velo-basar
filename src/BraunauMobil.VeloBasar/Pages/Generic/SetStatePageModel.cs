using BraunauMobil.VeloBasar.Logic.Generic;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Pages.Generic
{
    public class SetStateParameter : BasePageParameter
    {
        public ObjectState State { get; set; }
    }
    public class SetStatePageModel<TModel> : BasePageModel<TModel> where TModel : IModel, new()
    {
        private readonly ICrudContext<TModel> _context;

        public SetStatePageModel(ICrudContext<TModel> context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(SetStateParameter setStateParameter)
        {
            Parameter = setStateParameter ?? throw new ArgumentNullException(nameof(setStateParameter));

            if (await _context.ExistsAsync(Parameter.Id))
            {
                await _context.SetStateAsync(Parameter.Id, setStateParameter.State);
            }
            else
            {
                return NotFound();
            }
            return RedirectToListOrigin();
        }
    }
}
