using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Labels
{
    public class SelectAcceptanceModel : PageModel
    {
        [BindProperty]
        public int AcceptanceNumber { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            return this.RedirectToPage<CreateAndPrintForAcceptanceModel>(new CreateAndPrintForAcceptanceParameter { AcceptanceNumber = AcceptanceNumber });
        }
    }
}