using BraunauMobil.VeloBasar.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface ICountryContext
    {
        Task CreateAsync(Country country);
        SelectList GetSelectList();
    }
}
