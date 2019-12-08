using System.Diagnostics.Contracts;
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
            Contract.Requires(parameter != null);

            var file = await _context.GetAsync(parameter.FileId);
            if (file == null)
            {
                return NotFound();
            }

            return File(file.Data, file.ContentType);
        }
    }
}