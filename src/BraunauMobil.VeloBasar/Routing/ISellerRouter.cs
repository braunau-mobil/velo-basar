using BraunauMobil.VeloBasar.Parameters;

namespace BraunauMobil.VeloBasar.Routing;

public interface ISellerRouter
    : ICrudRouter<SellerEntity>
{
    string ToCreateForAcceptance();

    string ToCreateForAcceptance(int id);

    string ToSearchForAcceptance();

    string ToDetails(int id);

    string ToList(SellerListParameter parameter);

    string ToLabels(int id);

    string ToSettle(int id);

    string ToTriggerStatusPush(int id);
}
