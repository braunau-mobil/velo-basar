#warning    @todo
//using BraunauMobil.VeloBasar.Models.Interfaces;
//using BraunauMobil.VeloBasar.Parameters;
//using BraunauMobil.VeloBasar.Routing.Base;

//namespace BraunauMobil.VeloBasar.Tests.Controllers.CrudControllerTests;

//public class CrudRouterWrapper<TModel>
//    : ICrudRouterOLD<TModel>
//    where TModel : class, IModel, new()
//{
//    private readonly ICrudRouterOLD<TModel> _router;

//    public CrudRouterWrapper(ICrudRouterOLD<TModel> routerToWrap)
//    {
//        _router = routerToWrap ?? throw new ArgumentNullException(nameof(routerToWrap));
//    }

//    public string ToCreate()
//        => _router.ToCreate();

//    public string ToDelete(TModel model)
//        => _router.ToDelete(model);

//    public string ToDetails(TModel model)
//        => _router.ToDetails(model);

//    public string ToDisable(TModel model)
//        => _router.ToDisable(model);

//    public string ToEdit(TModel model)
//        => _router.ToEdit(model);

//    public string ToEnable(TModel model)
//        => _router.ToEnable(model);

//    public string ToList()
//        => _router.ToList();

//    public string ToList(ListParameter parameter)
//        => _router.ToList(parameter);

//    public string ToList(int pageIndex)
//        => _router.ToList(pageIndex);

//    public string ToList(int? pageSize, int pageIndex)
//        => _router.ToList(pageSize, pageIndex);
//}

