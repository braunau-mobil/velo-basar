// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages
{
    public class ShowFileParameter
    {
        public int FileId { get; set; }
    }

    public class ShowFileModel : PageModel
    {
        private readonly IFileStoreContext _context;

        public ShowFileModel(IFileStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(ShowFileParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            var file = await _context.GetAsync(parameter.FileId);
            if (file == null)
            {
                return NotFound();
            }

            return File(file.Data, file.ContentType);
        }
    }
}