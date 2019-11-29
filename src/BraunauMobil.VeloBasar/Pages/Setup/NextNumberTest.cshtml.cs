﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BraunauMobil.VeloBasar.Pages.Setup
{
    public class NextNumberTestModel : PageModel
    {
        private readonly IVeloContext _context;

        public NextNumberTestModel(IVeloContext context)
        {
            _context = context;
        }

        public TimeSpan Elapsed { get; private set; }
        public List<int> Numbers { get; } = new List<int>();
#if DEBUG
        public async Task OnGetAsync()
        {
            var basar = new Basar
            {
                Id = 1
            };

            var stopwatch = Stopwatch.StartNew();

            for (var count = 0; count < 1000; count++)
            {
                Numbers.Add(_context.Db.NextNumber(basar, TransactionType.Lock));
            }

            Elapsed = stopwatch.Elapsed;
        }
#endif
    }
}
