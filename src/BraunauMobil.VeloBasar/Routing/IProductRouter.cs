using BraunauMobil.VeloBasar.Parameters;
using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Routing;

public interface IProductRouter
{
    string ToDetails(int id);

    string ToEdit(int id);

    string ToLabel(int id);

    string ToList();

    string ToList(ListParameter parameter);

    string ToList(int pageIndex);

    string ToList(int? pageSize, int pageIndex);

    string ToList(ProductListParameter parameter);

    string ToLock(int id);

    string ToSetAsLost(int id);

    string ToUnlock(int id);
}
