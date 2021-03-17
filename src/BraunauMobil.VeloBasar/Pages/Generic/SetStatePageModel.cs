using BraunauMobil.VeloBasar.Logic.Generic;
using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Pages.Generic
{
    public class SetStateParameter
    {
        public int Id { get; set; }
        public ObjectState State { get; set; }
        public int PageIndex { get; set; }
    }
    public class SetStatePageModel<TModel, TListPageModel> : PageModel where TModel : IModel, new() where TListPageModel : PageModel
    {
        private readonly ICrudContext<TModel> _context;

        public SetStatePageModel(ICrudContext<TModel> context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(SetStateParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            if (await _context.ExistsAsync(parameter.Id))
            {
                await _context.SetStateAsync(parameter.Id, parameter.State);
            }
            else
            {
                return NotFound();
            }
            return this.RedirectToPage<TListPageModel>(new SearchAndPaginationParameter { PageIndex = parameter.PageIndex });
        }
    }
}
