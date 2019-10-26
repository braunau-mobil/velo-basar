using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Data;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Pages.Labels
{
    public class SelectAcceptanceModel : BasarPageModel
    {
        public SelectAcceptanceModel(VeloBasarContext context) : base(context)
        {
        }

        [BindProperty]
        public int AcceptanceNumber { get; set; }

        public async Task OnGetAsync(int basarId)
        {
            await LoadBasarAsync(basarId);
        }
        public async Task<IActionResult> OnPostAsync(int basarId)
        {
            await LoadBasarAsync(basarId);

            return RedirectToPage("/Labels/CreateAndPrintForAcceptance", new { basarId, acceptanceNumber = AcceptanceNumber });
        }
    }
}