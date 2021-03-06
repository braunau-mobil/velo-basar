﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BraunauMobil.VeloBasar.Logic;
using System;

namespace BraunauMobil.VeloBasar.Pages.ProductTypes
{
    public class DeleteParameter
    {
        public int ProductTypeId { get; set; }
        public int PageIndex { get; set; }
    }
    public class DeleteModel : PageModel
    {
        private IProductTypeContext _service;
        
        public DeleteModel(IProductTypeContext service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnGetAsync(DeleteParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            if (await _service.ExistsAsync(parameter.ProductTypeId))
            {
                await _service.DeleteAsync(parameter.ProductTypeId);
            }
            else
            {
                return NotFound();
            }
            return this.RedirectToPage<ListModel>(new SearchAndPaginationParameter { PageIndex = parameter.PageIndex });
        }
    }
}
