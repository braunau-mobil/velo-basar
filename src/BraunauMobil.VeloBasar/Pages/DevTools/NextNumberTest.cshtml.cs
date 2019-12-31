using System;
using System.Collections.Generic;
using System.Diagnostics;
using BraunauMobil.VeloBasar.Logic;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.DevTools
{
    public class NextNumberTestModel : PageModel
    {
        private readonly IVeloContext _context;
        private readonly INumberContext _numberContext;

        public NextNumberTestModel(IVeloContext context, INumberContext numberContext)
        {
            _context = context;
            _numberContext = numberContext;
        }

        public TimeSpan Elapsed { get; private set; }
        public List<int> Numbers { get; } = new List<int>();
        public IActionResult OnGet()
        {
            if (!_context.DevToolsEnabled())
            {
                return Unauthorized();
            }

            var basar = new Basar
            {
                Id = 1
            };

            var stopwatch = Stopwatch.StartNew();

            for (var count = 0; count < 1000; count++)
            {
                Numbers.Add(_numberContext.NextNumber(basar, TransactionType.Lock));
            }

            Elapsed = stopwatch.Elapsed;

            return Page();
        }
    }
}
