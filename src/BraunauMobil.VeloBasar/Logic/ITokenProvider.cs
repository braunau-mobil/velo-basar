using BraunauMobil.VeloBasar.Models;

namespace BraunauMobil.VeloBasar.Logic
{
    public interface ITokenProvider
    {
        string CreateToken(Seller seller);
    }
}
