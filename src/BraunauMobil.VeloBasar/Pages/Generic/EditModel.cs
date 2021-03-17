using BraunauMobil.VeloBasar.Logic.Generic;
using BraunauMobil.VeloBasar.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Pages.Generic
{
    public class EditParameter
    {
        public int Id { get; set; }
        public int PageIndex { get; set; }
    }
    public class EditModel<TModel, TListPageModel> : PageModel where TModel : IModel, new() where TListPageModel : PageModel
    {
        private readonly ICrudContext<TModel> _context;
        private int _pageIndex;

        public EditModel(ICrudContext<TModel> context)
        {
            _context = context;
        }

        [BindProperty]
        public TModel Item { get; set; }

        public string Title { get; set; }

        public async Task<IActionResult> OnGetAsync(EditParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            Item = await _context.GetAsync(parameter.Id);
            _pageIndex = parameter.PageIndex;

            if (Item == null)
            {
                return NotFound();
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(EditParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _context.UpdateAsync(Item);
            return this.RedirectToPage<TListPageModel>(new SearchAndPaginationParameter { PageIndex = _pageIndex });
        }
        public VeloPage GetListPage() => this.GetPage<TListPageModel>(new SearchAndPaginationParameter { PageIndex = _pageIndex });
    }
}
