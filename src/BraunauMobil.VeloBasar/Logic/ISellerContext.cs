using BraunauMobil.VeloBasar.Models;
using System.Linq;
using System.Threading.Tasks;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface ISellerContext
    {
        Task<Seller> CreateAsync(Seller toCreate);
        Task<bool> ExistsAsync(int id);
        Task<Seller> GetAsync(int id);
        IQueryable<Seller> GetMany(string searchString, ValueState? valueState);
        IQueryable<Seller> GetMany(string firstName, string lastName);
        Task SetValueStateAsync(int sellerId, ValueState valueState);
        Task UpdateAsync(Seller seller);
    }
}
