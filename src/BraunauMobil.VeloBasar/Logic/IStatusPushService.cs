using BraunauMobil.VeloBasar.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface IStatusPushService
    {
        Task PushAwayAsync(Basar basar, IEnumerable<Product> products);
    }
}
