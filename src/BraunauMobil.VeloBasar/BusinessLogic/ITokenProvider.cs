namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface ITokenProvider
{
    string CreateToken(SellerEntity seller);
}
