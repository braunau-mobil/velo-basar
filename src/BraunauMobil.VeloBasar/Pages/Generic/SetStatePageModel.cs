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
    public class SetStatePageModel<TModel> : BasePageModel<TModel> where TModel : IStateModel, new()
    {
        private readonly ICrudContext<TModel> _crudContext;
        private readonly IStateContext<TModel> _stateContext;

        public SetStatePageModel(ICrudContext<TModel> crudContext, IStateContext<TModel> stateContext)
        {
            _crudContext = crudContext;
            _stateContext = stateContext;
        }

        public async Task<IActionResult> OnGetAsync(SetStateParameter setStateParameter)
        {
            Parameter = setStateParameter ?? throw new ArgumentNullException(nameof(setStateParameter));

            if (await _crudContext.ExistsAsync(Parameter.Id))
            {
                await _stateContext.SetStateAsync(Parameter.Id, setStateParameter.State);
            }
            else
            {
                return NotFound();
            }
            return RedirectToListOrigin();
        }
    }
}
