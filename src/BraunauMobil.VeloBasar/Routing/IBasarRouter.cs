using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Routing;

public interface IBasarRouter
    : ICrudRouter
{
    string ToActiveBasarDetails();

    string ToDetails(int id);
}
