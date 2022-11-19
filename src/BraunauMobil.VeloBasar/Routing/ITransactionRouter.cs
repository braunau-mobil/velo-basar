using BraunauMobil.VeloBasar.Parameters;
using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Routing;

public interface ITransactionRouter
{
    string ToCancel(int id);

    string ToDetails(int id);

    string ToDocument(int id);

    string ToList();

    string ToList(int? pageSize, int pageIndex);

    string ToList(ListParameter parameter);

    string ToList(TransactionListParameter parameter);

    string ToList(TransactionType type);

    string ToSucess(int id);
}
