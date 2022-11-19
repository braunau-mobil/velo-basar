namespace BraunauMobil.VeloBasar.Routing;

public interface IAcceptProductRouter
{
    string ToCreate(int sessionId);

    string ToDelete(int sessionId, int productId);

    string ToEdit(int productId);
}