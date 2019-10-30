using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
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
        private readonly VeloBasarContext _context;

        public ShowFileModel(VeloBasarContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(ShowFileParameter parameter)
        {
            var file = await _context.FileStore.GetAsync(parameter.FileId);
            if (file == null)
            {
                return NotFound();
            }

            return File(file.Data, file.ContentType);
        }
    }
}