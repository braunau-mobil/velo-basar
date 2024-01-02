using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Parameters;
using Microsoft.AspNetCore.Routing;
using Xan.AspNetCore.Mvc;
using Xan.AspNetCore.Mvc.Routing;
using Xan.AspNetCore.Parameter;

namespace BraunauMobil.VeloBasar.Routing;

public sealed class TransactionRouter
    : ControllerRouter
    , ITransactionRouter
{
    public TransactionRouter(LinkGenerator linkGenerator)
        : base(MvcHelper.ControllerName<TransactionController>(), linkGenerator)
    { }

    public string ToCancel(int id)
        => GetUriByAction(nameof(TransactionController.Cancel), new { id });

    public string ToDetails(int id)
        => GetUriByAction(nameof(TransactionController.Details), new { id });

    public string ToDocument(int id)
        => GetUriByAction(nameof(TransactionController.Document), new { id });

    public string ToList(int? pageSize, int pageIndex)
    => ToList(new TransactionListParameter { PageIndex = pageIndex, PageSize = pageSize });

    public string ToList()
        => ToList(new TransactionListParameter());

    public string ToList(TransactionType type)
        => GetUriByAction(nameof(TransactionController.List), new TransactionListParameter { TransactionType = type });

    public string ToList(ListParameter parameter)
        => GetUriByAction(nameof(TransactionController.List), parameter);

    public string ToList(TransactionListParameter parameter)
        => GetUriByAction(nameof(TransactionController.List), parameter);

    public string ToSucess(int id)
        => GetUriByAction(nameof(TransactionController.Success), new { id });
}
