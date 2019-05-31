using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Data
{
    public class VeloBasarContext : DbContext
    {
        public VeloBasarContext (DbContextOptions<VeloBasarContext> options)
            : base(options)
        {
        }

        public DbSet<BraunauMobil.VeloBasar.Models.Basar> Basar { get; set; }
    }
}
