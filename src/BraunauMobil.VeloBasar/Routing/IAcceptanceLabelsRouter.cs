namespace BraunauMobil.VeloBasar.Routing;

public interface IAcceptanceLabelsRouter
{
    string ToDownload(int id);

    string ToSelect();
}