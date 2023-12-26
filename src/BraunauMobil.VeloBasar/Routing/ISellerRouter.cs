namespace BraunauMobil.VeloBasar.Routing;

public interface ISellerRouter
    : ICrudRouter
{
    string ToCreateForAcceptance();

    string ToCreateForAcceptance(int id);

    string ToSearchForAcceptance();

    string ToDetails(int id);

    string ToLabels(int id);

    string ToSettle(int id);

    string ToTriggerStatusPush(int id);
}
