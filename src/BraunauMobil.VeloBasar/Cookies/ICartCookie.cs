namespace BraunauMobil.VeloBasar.Cookies;

public interface ICartCookie
    : ICookie
{
    void ClearCart();

    IList<int> GetCart();

    void SetCart(IList<int> cart);
}
