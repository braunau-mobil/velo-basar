namespace BraunauMobil.VeloBasar.Routing;

public interface ICartRouter
{
    string ToAdd();

    string ToCheckout();

    string ToClear();

    string ToDelete(int productId);

    string ToIndex();
}
