using BraunauMobil.VeloBasar.Logic.Generic;
using BraunauMobil.VeloBasar.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Pages.Generic
{
    public class DeleteParameter
    {
        public int Id { get; set; }
        public int PageIndex { get; set; }
    }
    public class DeleteModel<TModel, TListPageModel> : PageModel where TModel : IModel, new() where TListPageModel : PageModel
    {
        private readonly ICrudContext<TModel> _context;

        public DeleteModel(ICrudContext<TModel> context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(DeleteParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            if (await _context.ExistsAsync(parameter.Id))
            {
                await _context.DeleteAsync(parameter.Id);
            }
            else
            {
                return NotFound();
            }
            return this.RedirectToPage<TListPageModel>(new SearchAndPaginationParameter { PageIndex = parameter.PageIndex });
        }
    }
}
