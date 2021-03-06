﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System;

namespace BraunauMobil.VeloBasar.Pages.Brands
{
    public class DeleteParameter
    {
        public int BrandId { get; set; }
        public int PageIndex { get; set; }
    }
    public class DeleteModel : PageModel
    {
        private readonly IBrandContext _context;

        public DeleteModel(IBrandContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(DeleteParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            if (await _context.ExistsAsync(parameter.BrandId))
            {
                await _context.DeleteAsync(parameter.BrandId);
            }
            else
            {
                return NotFound();
            }
            return this.RedirectToPage<ListModel>(new SearchAndPaginationParameter { PageIndex = parameter.PageIndex });
        }
    }
}
