using BraunauMobil.VeloBasar.Models;
using BraunauMobil.VeloBasar.Models.Interfaces;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic.Generic
{
    public interface IStateContext<TModel> where TModel : IStateModel
    {
        Task SetStateAsync(int id, ObjectState state);
    }
}
