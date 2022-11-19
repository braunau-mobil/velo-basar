using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Routing;

public interface IAcceptSessionRouter
{
    string ToCancel(int sessionId, bool returnToList = false);

    string ToList();

    string ToList(int pageIndex);

    string ToList(int? pageSize, int pageIndex);

    string ToList(ListParameter parameter);

    string ToStart();

    string ToStartForSeller(int sellerId);

    string ToSubmit(int sessionId);
}